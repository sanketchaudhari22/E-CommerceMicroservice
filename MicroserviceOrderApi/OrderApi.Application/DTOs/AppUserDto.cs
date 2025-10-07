﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public record AppUserDto(

        int Id,

        [Required] string UserName,

        [Required] string TelephoneNumber,

        [Required] string Address,

        [Required , EmailAddress] string Email,

        [Required] string Password,

        [Required] string Role

    );

}
