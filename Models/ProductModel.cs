﻿using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Tên Sản phẩm")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Mô tả Sản phẩm")]
		public string Slug { get; set; }
		public string Description { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Giá Sản phẩm")]
		public decimal Price { get; set; }
		public int BrandId { get; set; }
		public int CategoryId { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public string Image {  get; set; }
	}
}
