﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public  record OrderDetailsDto(

       [Required] int OrderId,

       [Required] int ProductId,

       [Required] int Client,

       [Required , EmailAddress] string Email,

       [Required] string ProductName,

       [Required] int PurchaseQuantity,

       [Required, DataType(DataType.Currency)] decimal UnitPrice,

       [Required, DataType(DataType.Currency)] decimal TotalPrice,

       [Required] DateTime OrderdDate
    );

}
