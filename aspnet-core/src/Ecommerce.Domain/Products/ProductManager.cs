using System;
using System.Threading.Tasks;
using Ecommerce.ProductCategories;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Ecommerce.Products;

public class ProductManager : DomainService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;

    public ProductManager(
        IRepository<Product> productRepository,
        IRepository<ProductCategory, Guid> productCategoryRepository
    )
    {
        _productCategoryRepository = productCategoryRepository;
        _productRepository = productRepository;
    }
    public async Task<Product> CreateAsync(Guid manufacturerId,
        string name, string code, string slug,
        ProductType productType, string sKU,
        int sortOrder, bool visibility,
        bool isActive, Guid categoryId,
        string seoMetaDescription, string description,
        double sellPrice)
    {
        if (await _productRepository.AnyAsync(x => x.Name == name))
        {
            throw new UserFriendlyException("Tên sản phẩm đã tồn tại", EcommerceDomainErrorCodes.ProductNameAlreadyExists);
        }

        if (await _productRepository.AnyAsync(x => x.Code == code))
        {
            throw new UserFriendlyException("Mã sản phẩm đã tồn tại", EcommerceDomainErrorCodes.ProductCodeAlreadyExists);
        }

        if (await _productRepository.AnyAsync(x => x.SKU == sKU))
        {
            throw new UserFriendlyException("Mã SKU sản phẩm đã tồn tại", EcommerceDomainErrorCodes.ProductSKUAlreadyExists);
        }

        var category =  await _productCategoryRepository.GetAsync(categoryId);

        return new Product(Guid.NewGuid(),manufacturerId,name,code,slug,productType,sKU,sortOrder,
            visibility,isActive,categoryId,seoMetaDescription,description,null,sellPrice, category?.Name,category?.Slug);
    }
    
}