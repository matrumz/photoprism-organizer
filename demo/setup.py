import http.client
import json
import os

from dataclasses import dataclass, is_dataclass, fields
from urllib.parse import urlparse

def dc_from_dict(cls, data):
	"""
	Recursively instantiate a dataclass from a dict, ignoring extra keys.
	"""
	if not is_dataclass(cls):
		return data
	kwargs = {}
	for f in fields(cls):
		value = data.get(f.name)
		if is_dataclass(f.type):
			value = dc_from_dict(f.type, value) if value is not None else None
		elif (getattr(f.type, '__origin__', None) is list and hasattr(f.type, '__args__')):
			item_type = f.type.__args__[0]
			value = [dc_from_dict(item_type, v) for v in value] if value is not None else []
		kwargs[f.name] = value
	return cls(**kwargs)

@dataclass
class Details:
	PhotoID: int
	Keywords: str
	KeywordsSrc: str
	Notes: str
	NotesSrc: str

@dataclass
class File:
	UID: str
	PhotoUID: str
	Name: str

@dataclass
class LabelInfo:
	ID: int
	UID: str
	Slug: str
	CustomSlug: str
	Name: str
	Priority: int
	Favorite: bool
	Description: str
	Notes: str

@dataclass
class PhotoLabel:
	PhotoID: int
	LabelID: int
	LabelSrc: str
	Uncertainty: int
	Label: LabelInfo

@dataclass
class Photo:
	ID: int
	UID: str
	Type: str
	Name: str
	OriginalName: str
	Favorite: bool
	Details: Details
	# Albums: list[Album]
	Files: list[File]
	Labels: list[PhotoLabel]
	DeletedAt: str

	def list_from_response(response):
		"""
		Convert a response from the API into a Photo list.
		"""
		if response.status != 200:
			raise ValueError(f"Error fetching photos: {response.status} {response.reason}")
		data = json.loads(response.read().decode('utf-8'))
		if not isinstance(data, list):
			raise ValueError("Expected a list of photos")
		return [dc_from_dict(Photo, item) for item in data]

	def from_response(response):
		"""
		Convert a response from the API into a single Photo instance.
		"""
		if response.status != 200:
			raise ValueError(f"Error fetching photo: {response.status} {response.reason}")
		data = json.loads(response.read().decode('utf-8'))
		return dc_from_dict(Photo, data)

def get_http_connection():
	"""
	Create an HTTP connection to the PhotoPrism server.
	"""
	url = os.environ.get("PHOTOPRISM_SITE_URL")
	if not url:
		raise ValueError("PHOTOPRISM_SITE_URL environment variable not set")
	parsed = urlparse(url)
	host = parsed.hostname
	port = parsed.port or 2342
	return http.client.HTTPConnection(host, port)

def default_headers():
	"""
	Return default headers for PhotoPrism HTTP requests.
	"""
	return {
		"Content-Type": "application/json",
		"Accept": "application/json"
	}

def run_index():
	"""
	Start indexing the PhotoPrism library.
	"""
	conn = get_http_connection()
	conn.request("POST", "/api/v1/index",
		headers = default_headers(),
		body = json.dumps({
			"cleanup": True,
			"rescan": True
		})
	)
	response = conn.getresponse()
	if response.status != 200:
		raise ValueError(f"Indexing failed: {response.status} {response.reason}")
	conn.close()

def get_photos():
	conn = get_http_connection()
	conn.request("GET", "/api/v1/photos?count=100&offset=0&order=name&merged=false&quality=1",
		headers = default_headers()
	)
	response = conn.getresponse()
	photos = Photo.list_from_response(response)
	conn.close()
	return photos

def get_photo_details(photo):
	"""
	Get details for a specific photo.
	"""
	conn = get_http_connection()
	conn.request("GET", f"/api/v1/photos/{photo.UID}",
		headers = default_headers()
	)
	response = conn.getresponse()
	photo = Photo.from_response(response)
	conn.close()
	return photo

def add_label(photo, label):
	"""
	Add a label to a photo.
	"""
	conn = get_http_connection()
	conn.request("POST", f"/api/v1/photos/{photo.UID}/label",
		headers = default_headers(),
		body = json.dumps(label)
	)
	response = conn.getresponse()
	if response.status != 200:
		raise ValueError(f"Failed to add label: {response.status} {response.reason}")
	conn.close()

def approve_photo(photo):
	"""
	Approve a photo.
	"""
	conn = get_http_connection()
	conn.request("POST", f"/api/v1/photos/{photo.UID}/approve",
		headers = default_headers()
	)
	response = conn.getresponse()
	if response.status != 200:
		raise ValueError(f"Failed to approve photo: {response.status} {response.reason}")
	conn.close()

run_index()

detailed_photos = [get_photo_details(photo) for photo in get_photos()]
for photo in detailed_photos:
	if photo.Name.isdigit() and int(photo.Name) % 2 == 0 and not any(label.Label.Name == "Even Number" for label in photo.Labels):
		add_label(photo, {
			"Name": "Even Number",
			"Priority": 1,
			"Favorite": False,
			"Description": "This photo has an even number as its name.",
		})
	elif photo.Name.isdigit() and int(photo.Name) % 2 != 0 and not any(label.Label.Name == "Odd Number" for label in photo.Labels):
		add_label(photo, {
			"Name": "Odd Number",
			"Priority": 1,
			"Favorite": False,
			"Description": "This photo has an odd number as its name.",
		})
	approve_photo(photo)

