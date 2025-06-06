﻿namespace WebApp.Models
{
	public class CartModel
	{
		public long ProductId {  get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public string Image {  get; set; }
		public decimal TotalPrice { 
			get { return Quantity * Price; } 
		}
		public CartModel()
		{

		}
		public CartModel(ProductModel product)
		{
			ProductId = product.Id;
			ProductName = product.Name;
			Price = product.Price;
			Quantity = 1;
			Image = product.Image;
		}
	}
}
