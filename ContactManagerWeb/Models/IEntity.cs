using System;

namespace ContactManagerWeb.Models
{
    public interface IEntity
    {
        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}