using dotnet.Catalog.Service.Dtos;
using dotnet.Catalog.Service.Entities;

namespace dotnet.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}