using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using portfolio_backend.Data;
using portfolio_backend.Dto;
using portfolio_backend.Models;

namespace portfolio_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DocsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Docs
        [HttpGet]
        public async Task<ActionResult<List<DocDto>>> GetDocs()
        {
            List<Doc> docList = await _context.Docs.ToListAsync();
            List<DocDto> docDtoList = [];

            foreach (Doc document in docList)
            {
                docDtoList.Add(new DocDto(document.Id, _context.Repositorys.Find(document.RepositoryId)?.Name ?? ""));
            }

            return docDtoList;
        }

        // GET: api/Docs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doc>> GetDoc(int id)
        {
            var doc = await _context.Docs.FindAsync(id);

            if (doc == null)
            {
                return NotFound();
            }

            return doc;
        }

        private bool DocExists(int id)
        {
            return _context.Docs.Any(e => e.Id == id);
        }
    }
}
