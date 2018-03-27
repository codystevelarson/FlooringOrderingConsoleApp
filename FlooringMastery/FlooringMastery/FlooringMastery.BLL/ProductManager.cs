using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class ProductManager
    {
        private IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ProductLookupResponse GetProduct(string productType)
        {
            ProductLookupResponse response = new ProductLookupResponse();
            if(productType == "")
            {
                response.Success = false;
                response.Message = "";
                return response;
            }

            response.Product = _productRepository.GetProduct(productType);
            if(response.Product == null)
            {
                response.Success = false;
                response.Message = $"Product info does not exist for {productType}";
                return response;
            }
            else
            {
                response.Success = true;
            }
            return response;
        }

        public ProductListResponse GetProductList()
        {
            ProductListResponse response = new ProductListResponse();
            response.Products = _productRepository.List();
            if(response.Products.Count() == 0)
            {
                response.Success = false;
                response.Message = "No products in product repository";
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
    }
}
