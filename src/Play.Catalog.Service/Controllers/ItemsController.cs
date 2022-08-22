using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using System.Linq;
using Play.Catalog.Service.Repositories;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        // private static readonly List<ItemDto> items = new() {
        //     new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Antibote", "Cures poison", 7, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow),
        // };

        private readonly IItemsRepository itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        // [HttpGet]
        // public IEnumerable<ItemDto> Get()
        // {
        //     return items;
        // }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            return items;
        }

        //Get/Items/{id}
        // [HttpGet("{id}")]
        // public ActionResult<ItemDto> GetById(Guid id)
        // {
        //     var item = items.Where(item => item.Id == id).SingleOrDefault();
        //     if (item == null)
        //     {
        //         return NotFound();
        //     }
        //     return item;
        // }

        //Get/Items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        //  [HttpPost]
        // public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        // {
        //     var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
        //     items.Add(item);
        //     return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        // }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var exisitngItem = await itemsRepository.GetAsync(id);

            if (exisitngItem == null)
            {
                return NotFound();
            }
            exisitngItem.Name = updateItemDto.Name;
            exisitngItem.Description = updateItemDto.Description;
            exisitngItem.Price = updateItemDto.Price;
            await itemsRepository.UpdateAsync(exisitngItem);
            return NoContent();
        }

        // [HttpPut("{id}")]
        // public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        // {
        //     var exisitngItem = items.Where(item => item.Id == id).SingleOrDefault();
        //     var updateItem = exisitngItem with
        //     {
        //         Name = updateItemDto.Name,
        //         Description = updateItemDto.Description,
        //         Price = updateItemDto.Price
        //     };

        //     var index = items.FindIndex(x => x.Id == id);
        //     if (index < 0)
        //     {
        //         return NotFound();
        //     }
        //     items[index] = updateItem;
        //     return NoContent();
        // }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await itemsRepository.RemoveAsync(item.Id);
            return NoContent();
        }

        //  [HttpDelete("{id}")]
        // public IActionResult Delete(Guid id)
        // {
        //     var index = items.FindIndex(item => item.Id == id);
        //     if (index < 0)
        //     {
        //         return NotFound();
        //     }
        //     items.RemoveAt(index);
        //     return NoContent();
        // }
    }
}