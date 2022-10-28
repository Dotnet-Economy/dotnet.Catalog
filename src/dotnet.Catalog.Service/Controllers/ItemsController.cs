using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet.Catalog.Contracts;
using dotnet.Catalog.Service.Dtos;
using dotnet.Catalog.Service.Entities;
using dotnet.Common;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace dotnet.Catalog.Service.Controllers
{

    [ApiController]
    // https://localhost:5001/items
    [Route("items")]
    //Only client with Role AdminRole is authed for access

    public class ItemsController : ControllerBase
    {
        private const string AdminRole = "Admin";
        private readonly IRepository<Item> itemsRepository;
        private readonly IPublishEndpoint publishEndpoint;
        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            this.itemsRepository = itemsRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [Authorize(Policies.Read)]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {

            var items = (await itemsRepository.GetAllAsync())
            .Select(item => item.AsDto());

            return Ok(items);
        }

        //GET /items/{id} e.g. /items/12345
        [HttpGet("{id}")]
        [Authorize(Policies.Read)]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        [Authorize(Policies.Write)]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Price = createItemDto.Price,
                Description = createItemDto.Description,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description, item.Price));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);

        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        [Authorize(Policies.Write)]
        public async Task<IActionResult> UpdateItemAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null) return NotFound();

            existingItem.Name = updateItemDto.Name;
            existingItem.Price = updateItemDto.Price;
            existingItem.Description = updateItemDto.Description;

            await itemsRepository.UpdateAsync(existingItem);

            await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description, existingItem.Price));

            return NoContent();
        }

        // DELETE items/{id}
        [HttpDelete("{id}")]
        [Authorize(Policies.Write)]
        public async Task<IActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null) return NotFound();

            await itemsRepository.RemoveAsync(existingItem.Id);

            await publishEndpoint.Publish(new CatalogItemDeleted(id));

            return NoContent();
        }

    }
}