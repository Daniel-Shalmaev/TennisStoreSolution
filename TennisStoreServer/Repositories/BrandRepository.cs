﻿using Microsoft.EntityFrameworkCore;
using TennisStoreServer.Data;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Repositories
{
    public class BrandRepository(AppDbContext appDbContext) : IBrand
    {
        private readonly AppDbContext appDbContext = appDbContext;

        public async Task<ServiceResponse> AddBrand(Brand model)
        {
            if (model is null) return new ServiceResponse(false, "Model is null");
            var (flag, meassage) = await CheckName(model.EnName!);
            if (flag)
            {
                appDbContext.Brands.Add(model);
                await Commit();
                return new ServiceResponse(true, "Brand Saved");
            }
            return new ServiceResponse(flag, meassage);
        }

        private async Task<ServiceResponse> CheckName(string name)
        {
            var brand = await appDbContext.Brands.FirstOrDefaultAsync(x => x.EnName!.ToLower()!.Equals(name.ToLower()));
            return brand is null ? new ServiceResponse(true, null!) :
                new ServiceResponse(false, "Category already exist");
        }

        public async Task<List<Product>> GetProductsByBrandAndSubcategory(int brandId, int subcategoryId)
        {
            return await appDbContext.Products
                .Where(p => p.BrandId == brandId && p.SubCategoryId == subcategoryId)
                .ToListAsync();
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();

        public async Task<List<Brand>> GetAllBrands() => await appDbContext.Brands.ToListAsync();

        public async Task<Brand?> GetBrandById(int id) =>
            await appDbContext.Brands.Include(b => b.Subcategories)!.ThenInclude(sc => sc.Category).FirstOrDefaultAsync(b => b.Id == id);

    }
}
