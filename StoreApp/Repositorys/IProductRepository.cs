using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreApp.Models;

namespace StoreApp.Repositorys
{
    public interface IProductRepository
    {
        Product GetById(int id);
        bool Add(Product product);
        bool Delete(int id); 
    }
}