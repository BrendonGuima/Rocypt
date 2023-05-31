﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocypt.Data;
using Rocypt.Models;
using Rocypt.Repositorio;
using System.Security.Claims;

namespace Rocypt.Controllers
{
    public class PainelController : Controller
    {
        private readonly IGrupoRepositorio _grupoRespositorio;
        private readonly DatabankContext _databankContext;


        public PainelController(IGrupoRepositorio grupoRepositorio, DatabankContext databankContext)
        {
            _grupoRespositorio = grupoRepositorio;
            _databankContext = databankContext;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Index", "Login");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = new PainelIndexModel();
            model.grupoModels = _grupoRespositorio.BuscarTodos(Guid.Parse(userId));
            return View(model);
        }


        public IActionResult Deslogar()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult CriarGrupo(GrupoModel grupo)
        {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    UsuarioModel usuario = _databankContext.Usuarios.Find(Guid.Parse(userId));
                    grupo.UsuarioId = usuario.Id;
                    grupo = _grupoRespositorio.Adicionar(grupo);
                    return RedirectToAction("Index");
                }
                return View();   
        }

        [HttpPost]
        public IActionResult AlterarGrupo(GrupoModel grupo)
        {
            
            if (ModelState.IsValid)
            {
                grupo = _grupoRespositorio.Atualizar(grupo);
                _databankContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }



        [HttpPost]
        public IActionResult ApagarGrupo(GrupoModel grupo)
        {
            if (ModelState.IsValid)
            {
                grupo = _grupoRespositorio.Apagar(grupo);
                _databankContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}
