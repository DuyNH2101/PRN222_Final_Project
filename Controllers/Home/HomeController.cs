﻿using E_Retalling_Portal.Models;
using E_Retalling_Portal.Models.Enums;
using E_Retalling_Portal.Models.Query;
using E_Retalling_Portal.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using X.PagedList.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Retalling_Portal.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Error505()
        {
            return View("Views/Shared/ErrorPage/Error500.cshtml");
        }
        public IActionResult Index(string? searchQuery, int? categoryId, double? minPrice, double? maxPrice, int? page)
        {
            using (var context = new Context())
            {
                var imageList = context.Images.ToList();
                var productList = context.Products.GetProduct().ToList();
                var categoryList = context.Categories.GetCategories().ToList();
                var subcategoryList = context.Categories.GetSubCategories().ToList();
                List<BreadcrumbItem> breadcrumbList = new List<BreadcrumbItem>();
                breadcrumbList = GetBreadcrumListFromCategoryList(categoryList, categoryId, breadcrumbList, context);

                ViewBag.breadcrumbList = breadcrumbList;

                //Get subcategory by categoryId
                if (categoryId.HasValue)
                {
                    ViewBag.isParentCategory = false;
                    ViewBag.subcategory = context.Categories.GetSubCategoriesByParentCategoryId(categoryId).ToList();
                }
                else
                {
                    ViewBag.isParentCategory = true;
                    ViewBag.categoryList = categoryList;
                }

                if (categoryId.HasValue)
                {
                    productList = GetProductsBySubCategories(categoryId, categoryList, productList, context);
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    productList = GetProductsByFilterSearch(productList, searchQuery);
                }

                if (minPrice.HasValue || maxPrice.HasValue)
                {
                    productList = GetProductsByPrice(minPrice, maxPrice, productList);
                }


                var productDiscountItem = new Dictionary<int, ProductDiscountItemModel>();
                //Get min price of productItems
                if (productList != null)
                {
                    foreach (var product in productList)
                    {
                        if (product.isVariation == true && product.productItems.Count > 0)
                        {
                            foreach(var item in product.productItems)
                            {
                                if (!productDiscountItem.ContainsKey(product.id))
                                {
                                    productDiscountItem[product.id] = new ProductDiscountItemModel();
                                }
                                var productDiscount = context.ProductDiscounts.GetProductDiscountByProductId(product.id).FirstOrDefault();
                                if (productDiscount != null)
                                {
                                    productDiscountItem[product.id].productItem = item;
                                    productDiscountItem[product.id].productDiscount = productDiscount;
                                    productDiscountItem[product.id].discountedPrice = context.ProductItems.GetProductItemDiscountPrice(item);
                                    if (item.price != productDiscountItem[product.id].discountedPrice)
                                    {
                                        productDiscountItem[product.id].isDiscount = "true";
                                    }
                                    else
                                    {
                                        productDiscountItem[product.id].isDiscount = "false";
                                    }
                                    if(productDiscountItem[product.id].isDiscount == "true")
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!productDiscountItem.ContainsKey(product.id))
                            {
                                productDiscountItem[product.id] = new ProductDiscountItemModel();
                            }
                            var productDiscount = context.ProductDiscounts.GetProductDiscountByProductId(product.id).FirstOrDefault();
                            if (productDiscount != null)
                            {
                                productDiscountItem[product.id].product = product;
                                productDiscountItem[product.id].productDiscount = productDiscount;
                                productDiscountItem[product.id].discountedPrice = context.Products.GetProductDiscountPrice(product);
                                if (product.price != productDiscountItem[product.id].discountedPrice)
                                {
                                    productDiscountItem[product.id].isDiscount = "true";
                                }
                                else
                                {
                                    productDiscountItem[product.id].isDiscount = "false";
                                }
                            }
                        }
                    }
                }
                foreach (var entry in productDiscountItem)
                {
                    // In key
                    Console.WriteLine($"Key: {entry.Key}");
                    Console.WriteLine($"Discounted Price: {entry.Value.isDiscount}");
                    // In các giá trị trong ProductDiscountItemModel
                    Console.WriteLine($"Discounted Price: {entry.Value.discountedPrice}");
                    if (entry.Value.product != null)
                    {
                        Console.WriteLine($"Product Price: {entry.Value.product.price}");
                    }
                    if (entry.Value.productItem != null)
                    {
                        Console.WriteLine($"productItem Price: {entry.Value.productItem.price}");
                    }
                    if (entry.Value.productDiscount != null)
                    {
                        Console.WriteLine($"Product Discount: {entry.Value.productDiscount.discount.value}");
                    }
                    Console.WriteLine("-----------------------------");
                }
                if (productList != null)
                {
                    foreach (var product in productList)
                    {
                        if (product.isVariation == true && product.productItems.Count > 0)
                        {
                            var min = product.productItems.Min(item => item.price);

                            product.price = min;
                        }
                    }
                }
                var pageNumber = page ?? 1;
                var pageSize = 24;


                List<Product> products = GetProductsIsNotDelete(productList);

                var paginatedProducts = products.ToPagedList(pageNumber, pageSize);

                
                ViewBag.productDiscounts = productDiscountItem;
                ViewBag.searchQuery = searchQuery;
                ViewBag.imageList = imageList;
                ViewBag.categoryId = categoryId;
                ViewBag.minPrice = minPrice;
                ViewBag.maxPrice = maxPrice;
                ViewBag.currentPage = pageNumber;
                ViewBag.recommendedProduct = GetRecommendProduct();
                ViewBag.page = pageNumber;
                var productAverageRatings = new Dictionary<int, int>();
                var recommendProductRatings = new Dictionary<int, int>();
                foreach (var product in paginatedProducts)
                {
                    // Get the ratings for the current product
                    var ratings = context.OrderItems
                        .Where(oi => oi.productId == product.id && oi.rating.HasValue)
                        .Select(oi => oi.rating.Value)
                        .ToList();

                    // Calculate the average rating for the current product, rounding to the nearest integer
                    int averageRating = ratings.Any() ? (int)Math.Round(ratings.Average()) : 0;

                    // Add the productId and its average rating to the dictionary
                    productAverageRatings[product.id] = averageRating;
                }
                foreach (var recommendProduct in GetRecommendProduct())
                {
                    // Get the ratings for the current product
                    var ratings = context.OrderItems
                        .Where(oi => oi.productId == recommendProduct.id && oi.rating.HasValue)
                        .Select(oi => oi.rating.Value)
                        .ToList();

                    // Calculate the average rating for the current product, rounding to the nearest integer
                    int recommendaverageRating = ratings.Any() ? (int)Math.Round(ratings.Average()) : 0;

                    // Add the productId and its average rating to the dictionary
                    recommendProductRatings[recommendProduct.id] = recommendaverageRating;
                }
                ViewBag.productAverageRatings = productAverageRatings;
                ViewBag.recommendProductRatings = recommendProductRatings;
                return View(paginatedProducts);          
            }
        }
        public IActionResult ViewShop(int id, int? page)
        {
            List<Product> productList;
            Shop shop;
            using (var context = new Context())
            {
                productList = context.Products.GetProduct().Where(p=>p.shopId==id).ToList();

                var productDiscountItem = new Dictionary<int, ProductDiscountItemModel>();
                if (productList != null)
                {
                    foreach (var product in productList)
                    {
                        if (product.isVariation == true && product.productItems.Count > 0)
                        {
                            foreach (var item in product.productItems)
                            {
                                if (!productDiscountItem.ContainsKey(product.id))
                                {
                                    productDiscountItem[product.id] = new ProductDiscountItemModel();
                                }
                                var productDiscount = context.ProductDiscounts.GetProductDiscountByProductId(product.id).FirstOrDefault();
                                if (productDiscount != null)
                                {
                                    productDiscountItem[product.id].productItem = item;
                                    productDiscountItem[product.id].productDiscount = productDiscount;
                                    productDiscountItem[product.id].discountedPrice = context.ProductItems.GetProductItemDiscountPrice(item);
                                    if (item.price != productDiscountItem[product.id].discountedPrice)
                                    {
                                        productDiscountItem[product.id].isDiscount = "true";
                                        break;
                                    }
                                    else
                                    {
                                        productDiscountItem[product.id].isDiscount = "false";
                                    }
                                    if (productDiscountItem[product.id].isDiscount == "true")
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!productDiscountItem.ContainsKey(product.id))
                            {
                                productDiscountItem[product.id] = new ProductDiscountItemModel();
                            }
                            var productDiscount = context.ProductDiscounts.GetProductDiscountByProductId(product.id).FirstOrDefault();
                            if (productDiscount != null)
                            {
                                productDiscountItem[product.id].product = product;
                                productDiscountItem[product.id].productDiscount = productDiscount;
                                productDiscountItem[product.id].discountedPrice = context.Products.GetProductDiscountPrice(product);
                                if (product.price != productDiscountItem[product.id].discountedPrice)
                                {
                                    productDiscountItem[product.id].isDiscount = "true";
                                }
                                else
                                {
                                    productDiscountItem[product.id].isDiscount = "false";
                                }
                            }
                        }
                    }
                }
                ViewBag.productDiscounts = productDiscountItem;
                shop = context.Shops.Include(s=>s.products).Include(s=>s.account).ThenInclude(a=>a.user).FirstOrDefault(s => s.id == id);
            }
            if (productList != null)
            {
                foreach (var product in productList)
                {
                    if (product.isVariation == true && product.productItems.Count > 0)
                    {
                        var min = product.productItems.Min(item => item.price);

                        product.price = min;
                    }
                }
            }

            var pageNumber = page ?? 1;
            var pageSize = 42;
            List<Product> products = GetProductsIsNotDelete(productList);
            var paginatedProducts = products.ToPagedList(pageNumber, pageSize);
            ViewBag.shop = shop;
            ViewBag.id = id;
            return View("ViewShop", paginatedProducts);
        }

        private List<Product> GetProductsIsNotDelete(List<Product>? productList)
        {
            List<Product> products = new List<Product>();
            foreach (var product in productList)
            {
                if (product.isVariation == true)
                {
                    foreach (var productItem in product.productItems)
                    {
                        if (productItem.deleteAt == null)
                        {
                            products.Add(product);
                            break;
                        }
                    }
                }
                else
                {
                    products.Add(product);
                }
            }
            return products;
        }
        private List<Product> GetProductsByPrice(double? minPrice, double? maxPrice, List<Product> productList)
        {
            if (productList == null || !productList.Any())
            {
                return new List<Product>(); 
            }

            var filteredProducts = productList.Where(p => p.deleteAt == null);

            if (minPrice >= 2 && maxPrice < 1000)
            {                return filteredProducts.Where(p => p.price >= minPrice.Value && p.price < maxPrice.Value).ToList();
            }
            else if (minPrice >= 2 && maxPrice >= 1000)
            {
                return filteredProducts.Where(p => p.price >= minPrice.Value).ToList();
            }

            return filteredProducts.ToList();
        }

        private List<BreadcrumbItem> GetBreadcrumListFromCategoryList(List<Category> categoryList, int? categoryId, List<BreadcrumbItem> breadcrumbList, Context context)
        {
            var currentCategory = context.Categories.GetSubCategoriesByCategoryId(categoryId).FirstOrDefault();

            while (currentCategory != null)
            {
                breadcrumbList.Insert(0, new BreadcrumbItem
                {
                    Name = currentCategory.name,
                    Url = currentCategory.id.ToString()
                });
                currentCategory = context.Categories.GetSubCategoriesByCategoryId(currentCategory.parentCategoryId).FirstOrDefault();
            }
            return breadcrumbList;
        }

        private List<Product> GetProductsByFilterSearch(List<Product> productList, string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                return productList.Where(p => p.name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) && p.deleteAt == null)
                        .ToList();
            }
            return productList;
        }


        private List<Product> GetProductsBySubCategories(int? categoryId, List<Category> categoryList, List<Product> productList, Context context)
        {
            if (!categoryId.HasValue)
            {
                return new List<Product>();
            }

            List<int> subCategoryIds = new List<int> { categoryId.Value };

            var childCategories = categoryList.Where(c => c.parentCategoryId == categoryId.Value && c.deleteAt == null).ToList();

            foreach (var category in childCategories)
            {
                subCategoryIds.Add(category.id);

                subCategoryIds.AddRange(GetProductsBySubCategories(category.id, categoryList, productList, context)
                    .Select(p => p.categoryId));
            }

            return productList.Where(p => subCategoryIds.Contains(p.categoryId)).ToList();
        }

        private List<Product> GetRecommendProduct()
        {
            Dictionary<Product, double> recommendedProducts = new Dictionary<Product, double>();
            List<Product> recentProduct = new List<Product>();
            List<int> recentProductIds = Util.CookiesUtils.GetRecentlyViewedProductsFromCookie(Request, Response);
            using (var context = new Context())
            {
                foreach (var id in recentProductIds)
                {
                    var recentProd = context.Products.GetProductById(id).FirstOrDefault();
                    if (recentProd != null)
                    {
                        recentProduct.Add(recentProd);
                    }
                }
                //cal point
                var allProducts = GetProductsIsNotDelete(context.Products.GetProduct().ToList());
                foreach (var product in allProducts)
                {
                    double point = 0;
                    foreach (var recentProd in recentProduct)
                    {
                        point += CompareSemantics(product.id, recentProd.id);
                    }
                    //reduce point if already saw
                    if(recentProductIds.Contains(product.id)){
                        point -= 0.5;
                    }
                    recommendedProducts[product]=point;
                }
            }
            foreach (var product in recommendedProducts.Keys)
            {
                if (product.isVariation == true && product.productItems.Count > 0)
                {
                    var min = product.productItems.Min(item => item.price);

                    product.price = min;
                }
            }
            return recommendedProducts
                    .OrderByDescending(p => p.Value)
                    .Take(6)
                    .Select(p => p.Key)
                    .ToList();
        }
        private static double CompareSemantics(int productId1, int productId2)
        {
            var CosineSimilarity = 0.0;
            using (var context = new Context())
            {
                var p1 = context.Products.GetProductById(productId1).FirstOrDefault();
                var p2 = context.Products.GetProductById(productId2).FirstOrDefault();
                CosineSimilarity = VectorUtils.CosineSimilarity(p1.vectorEmbadding, p2.vectorEmbadding);
            }
            return CosineSimilarity;
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}