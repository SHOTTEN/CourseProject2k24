using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DishesApplication.Tools
{
	public class ProductBlank
	{
		public string ProductArticleNumber { get; set; }
		public string ProductName { get; set; }
		public string ProductDescription { get; set; }
		public Nullable<int> ProductCategoryId { get; set; }
		public string ProductPhoto { get; set; }
		public Nullable<int> ManufacturerId { get; set; }
		public Nullable<int> ProviderId { get; set; }
		public string ProductCost { get; set; }
		public string ProductDiscountAmount { get; set; }
		public string MaxDiscount { get; set; }
		public string CurrentDiscount { get; set; }
		public Nullable<int> ProductQuantityInStock { get; set; }

		public ProductBlank(string productArticleNumber, string productName, string productDescription,
			int? productCategoryId, string productPhoto, int? manufacturerId, int? providerId,
			string productCost, string productDiscountAmount, string maxDiscount, string currentDiscount, int? productQuantityInStock)
		{
			ProductArticleNumber = productArticleNumber;
			ProductName = productName;
			ProductDescription = productDescription;
			ProductCategoryId = productCategoryId;
			ProductPhoto = productPhoto;
			ManufacturerId = manufacturerId;
			ProviderId = providerId;
			ProductCost = productCost;
			ProductDiscountAmount = productDiscountAmount;
			MaxDiscount = maxDiscount;
			CurrentDiscount = currentDiscount;
			ProductQuantityInStock = productQuantityInStock;
		}

		public static ProductBlank Empty()
		{
			return new ProductBlank (null, null, null, null, null, null, null, null, null, null, null, null);
		}

	}
}
