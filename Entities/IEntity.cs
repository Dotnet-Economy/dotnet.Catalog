using System;

namespace dotnet.Catalog.Service.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }
}