# How to create a `.portfolio` folder

This document is for AI agents (and humans) working in **other** repositories
that should show up on Louis Venhoff's portfolio site. It explains exactly
what this backend (`portfolio_backend`) expects to find in a repo so it gets
picked up and rendered as a project.

## How the sync works (context)

On a webhook trigger, the backend:

1. Fetches all repos owned by the configured GitHub account.
2. Clones/pulls each repo to the server.
3. Looks for a folder named **`.portfolio`** at the **repository root**.
4. If `.portfolio/portfolio.md` exists, it creates/updates a "Doc" entry for
   that repo and reads the optional thumbnail and config files described
   below. **If `portfolio.md` is missing, the repo is skipped entirely** —
   nothing else in `.portfolio/` matters until that file exists.

## Folder layout

Create this at the root of the target repository:

```
<repo-root>/
└── .portfolio/
    ├── portfolio.md     # REQUIRED
    ├── config.json       # STRONGLY RECOMMENDED (see below)
    └── thumbnail.png      # OPTIONAL (.png, .jpg, or .jpeg)
```

### `portfolio.md` (required)

Plain Markdown. Its raw contents are served as-is and rendered as the
project's description/body on the site. Write whatever you'd want a visitor
to read about the project (what it is, how it works, tech used, screenshots
via relative/absolute image links, etc.). There is no required structure —
it's freeform Markdown.

### `config.json` (optional, but treat as required)

Without this file, the project's display name stays the literal placeholder
string `"DocumentName"` and its description is empty — not something you
want live on the site. Always include it.

Schema (property names are **case-sensitive**, must match exactly):

```json
{
  "DocumentName": "My Project Title",
  "Description": "A short one- or two-sentence summary of the project.",
  "Tags": ["C#", "React", "PostgreSQL"]
}
```

- `DocumentName` (string) — the project title shown in the UI.
- `Description` (string) — short blurb shown in the project list.
- `Tags` (array of strings) — **must always be present, even if empty
  (`[]`)**. A missing/`null` `Tags` field crashes the sync job for this repo.

Other rules to keep in mind:
- The JSON must be strictly valid — a parse error crashes the sync job.
- Tags are additive across syncs: once a tag has been added it is never
  removed automatically, even if you later delete it from `config.json`.
  Only add tags you actually want to persist.

### `thumbnail.{png,jpg,jpeg}` (optional)

A single image file named exactly `thumbnail` with one of these three
extensions. Any other name or format (e.g. `.webp`, `.gif`, `cover.png`) is
ignored. If present, it's used as the project's thumbnail image on the site.

## Minimal example

```
.portfolio/
├── portfolio.md
└── config.json
```

`config.json`:

```json
{
  "DocumentName": "Portfolio Sync Service",
  "Description": "Backend service that syncs GitHub repos into portfolio entries.",
  "Tags": ["C#", ".NET", "GitHub API"]
}
```

`portfolio.md`:

```markdown
# Portfolio Sync Service

A .NET backend that scans a GitHub account's repositories for `.portfolio`
folders and turns them into portfolio entries in a database.

## Highlights
- Automatic repo discovery via the GitHub API
- Markdown-based project descriptions
- Tag-based filtering on the frontend
```

## Filenames are case-sensitive

The sync runs on a Linux server, so all filenames must match exactly in
lowercase: `.portfolio`, `portfolio.md`, `config.json`, `thumbnail.png` (etc).
`Config.json` or `Thumbnail.PNG` will look fine on macOS/Windows but will be
silently ignored on the server.

## Checklist before pushing

- [ ] `.portfolio/` exists at the repo root (not nested elsewhere)
- [ ] `.portfolio/portfolio.md` exists and has real content
- [ ] `.portfolio/config.json` exists, is valid JSON, and includes
      `DocumentName`, `Description`, and a `Tags` array (use `[]` if you have
      no tags)
- [ ] (optional) `.portfolio/thumbnail.png` / `.jpg` / `.jpeg` added if you
      want a custom thumbnail
