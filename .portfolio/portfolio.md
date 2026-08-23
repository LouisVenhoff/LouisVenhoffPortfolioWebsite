# Automatic Portfolio Website

This project is Louis Venhoff's personal portfolio site — and the very
system that generates its own project list. Rather than hand-writing project
entries, the backend scans every GitHub repository in the account, looks for
a `.portfolio` folder, and turns it into a project card and detail page on
the site automatically. (This entry is a live example of that — it was
generated from this repo's own `.portfolio` folder.)

## How it works

1. A webhook triggers a sync job on the backend.
2. The backend fetches the full list of repos from the GitHub API and
   clones/pulls each one to the server.
3. Any repo containing a `.portfolio/portfolio.md` file is turned into a
   project entry ("Doc") in the database.
4. An optional `.portfolio/config.json` supplies the project's title,
   description, and tags, and an optional `thumbnail.{png,jpg,jpeg}` supplies
   a cover image.
5. The React frontend fetches the resulting list of projects and renders
   each one, lazily loading its Markdown description and thumbnail on
   demand.

## Tech stack

**Backend** (`portfolio_backend/`)
- ASP.NET Core 8 Web API
- Entity Framework Core with a MySQL/Pomelo provider
- LibGit2Sharp for cloning/pulling repositories server-side
- GitHub REST API for repository discovery
- GraphQL client used for supplementary data (e.g. commit heatmap)

**Frontend** (`louis_venhoff_portfolio/`)
- React 19 + TypeScript, built with Vite
- Chakra UI + Tailwind CSS for styling
- `react-markdown` / `@uiw/react-markdown-preview` to render each
  project's `portfolio.md`
- `react-calendar-heatmap` for a GitHub-style contribution graph
- `motion` for animations

**Infra**
- Dockerized (see `dockerfile`, `docker-compose.yml`, `docker-swarm.yml`)
- Deployed behind CORS configuration in `portfolio_backend/Helpers/CorsHelper.cs`

## Highlights

- Zero-touch project publishing: adding a project to the portfolio is just
  adding a `.portfolio` folder to a repo and pushing — no manual CMS entry.
- Tag-based project metadata persisted additively across syncs.
- See `PORTFOLIO_FOLDER_GUIDE.md` in this repo for the full spec any repo
  needs to follow to be picked up by the sync.
