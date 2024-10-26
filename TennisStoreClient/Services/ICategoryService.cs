﻿using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public interface ICategoryService
    {
        Action? CategoryAction {get;set;}
        public List<Category> AllCategories { get; set; }
        Task GetAllCategories();
        Task<ServiceResponse> AddCategory(Category model);
    }
}
