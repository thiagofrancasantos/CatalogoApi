﻿using ApiCatalogo.Infra;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controller;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    public ProdutosController(IProdutoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetProdutos().ToList();
        if(produtos is null)
        {
            return NotFound();
        }
        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name="ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.GetProduto(id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {

        if (produto is null) {
            return BadRequest();
        }

        var novoProduto = _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", 
            new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto) {
        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }

        bool atualizado = _repository.Update(produto);

        if (atualizado)
        {
            return Ok(produto);
        }
        else
        {
            return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");
        }

    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id){
       bool deletado = _repository.Delete(id);

        if (deletado)
        {
            return Ok($"Produto de id={id} foi excluido");
        }
        else
        {
            return StatusCode(500, $"Falha ao excluir o produto de id={id}");
        }

    }

}
