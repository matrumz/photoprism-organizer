You are assisting a senior lead engineer to author a design document in [design.md](vscode-file://vscode-app/c:/Users/matru/AppData/Local/Programs/Microsoft%20VS%20Code/resources/app/out/vs/code/electron-browser/workbench/workbench.html) for the photoprism-organizer repo. The document will be consumed by Blazor full stack engineers and, later, by an agentic AI project manager to infer work done/remaining; however, for this iteration, focus only on the design content (not on AI PM prompts, process conventions, or automation).

Objectives:

- Create a concise, operator-manual-style Preface that helps both humans and AI quickly understand how to read and use the document.
- Provide a primary software architecture diagram and a maintainable structure around it that the lead engineer can update over time.
- Add a machine-readable Architecture Index (YAML) that maps components to code paths and interfaces (no stable IDs).
- Keep diagram formatting in Mermaid within Markdown; do not add rendering steps (GitHub renders Mermaid).
- Leave diagram methodology (C4 levels, layering, or other) as an explicit open question to be resolved collaboratively as the design matures.
- Do not add ADRs or process details; templates in .github/ may be addressed in a future iteration.

Deliverables for this iteration (draft only; do not modify files yet):

1. Preface (operator manual style)
    - Audience: Blazor full stack engineers; also readable by AI.
    - Purpose and scope: how to navigate the design, where to find code, how updates should be made.
    - Conventions: headings, Mermaid sections, and YAML blocks for machine-readability; no stable IDs—use human-readable names.
    - Change guidance: where to add new components/sections and how to keep the diagram and YAML in sync.
2. Primary architecture diagram (Mermaid)
    - A starter diagram embedded in Markdown.
    - Names align with repository structure and component names under src/.
    - Keep methodology open (note an “Open Questions” subsection to choose between C4 levels or a simpler layered view).
3. Architecture Index (YAML)
    - A short, machine-readable section mapping component names → code\_paths → interfaces → owners → status/notes.
    - No stable IDs; use human-readable names only.
4. Open Questions section
    - Capture pending decisions: diagram methodology/levels, component granularity, and how deeply to map interfaces.
5. Minimal notes for future iterations (not implemented now)
    - Acknowledge that .github templates are acceptable but will be handled later alongside AI PM integration and repo/process details.

Style and constraints:

- Operator-manual tone: concise, actionable, and skimmable.
- Use Mermaid for diagrams; YAML for indexes; keep both embedded in docs/design.md.
- Avoid stable IDs.
- No ADRs.
- No compliance/PII constraints to consider.
- Do not apply any repository changes in this iteration; produce draft content only.

Assumptions (only if needed and explicitly stated):

- Component names mirror namespaces/folders in src/.
- Owners can be left as “TBD” for now if not known.

Now, produce:

- A proposed Preface draft (operator-manual style).
- A starter Mermaid diagram block.
- An Architecture Index YAML skeleton aligned to the repo layout.
- An Open Questions list capturing the diagram methodology decision and any other key unknowns.
- Brief guidance on how the lead engineer should maintain the diagram/YAML over time.

Open questions to confirm as we proceed (no blockers for drafting):

- Diagram methodology: Start with C4 (Context/Container/Component) levels or a simpler layered diagram? Which levels should we include first?
- Component granularity: Do you want to map down to individual SDK classes (e.g., PhotoPrism.Sdk.Client) or stop at project-level components initially?
- Ownership: Should we include owners now (by team/role) or mark as TBD?
