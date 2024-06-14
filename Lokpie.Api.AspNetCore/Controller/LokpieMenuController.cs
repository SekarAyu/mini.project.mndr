using Lokpie.Common.Commands;
using Lokpie.Common.Dtos;
using Lokpie.Common.Queries;
using Lokpie.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QSI.Common.Web;
using System.Collections.Generic;

namespace Lokpie.Api.AspNetCore.Controller
{
    [Authorize]
    [ApiController]
    [AllowAnonymous]
    [Route("Lokpie/Menu")]
    public class LokpieMenuController : ControllerBase
    {
        private readonly IMenuService menuService;

        public LokpieMenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        /// <summary>
        /// Get all menu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageStartRow}/{pageSize}")]
        [ProducesResponseType(typeof(IList<FoodNBevDto>), 200)]
        public ActionResult<DataTable<FoodNBevDto>> GetAllFoodNBev([FromQuery] FoodNBevQuery query, int pageStartRow, int pageSize)
        {
            return Ok(menuService.GetAllFoodNBev(query, pageStartRow, pageSize));
        }

        /// <summary>
        /// Get menu detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("detail")]
        [ProducesResponseType(typeof(FoodNBevDetailDto), 200)]
        public ActionResult<FoodNBevDetailDto> GetFoodNBevDetail(long menuId)
        {
            return Ok(menuService.GetFoodNBevDetail(menuId));
        }

        /// <summary>
        /// Get menu photo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("photo")]
        [ProducesResponseType(typeof(BasePhotoDto), 200)]
        public ActionResult<BasePhotoDto> GetFoodNBevPhoto([FromQuery] long idMenu)
        {
            return Ok(menuService.GetFoodNBevPhoto(idMenu));
        }

        /// <summary>
        /// Add new menu
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="AuthenticationException"></exception>
        [HttpPost]
        [ProducesResponseType(typeof(long), 200)]
        public long AddNewFoodNBev([FromBody] AddFoodNBevCommand command)
        {
            return menuService.AddNewFoodNBev(command);
        }

        /// <summary>
        /// Update menu
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="AuthenticationException"></exception>
        [HttpPut]
        [ProducesResponseType(typeof(long), 200)]
        public long UpdateFoodNBev([FromBody] UpdateFoodNBevCommand command)
        {
            return menuService.UpdateFoodNBev(command);
        }

        /// <summary>
        /// Update menu
        /// </summary>
        /// <exception cref="AuthenticationException"></exception>
        [HttpDelete]
        [ProducesResponseType(typeof(long), 200)]
        public void DeleteFoodNBev([FromQuery] long id)
        {
            menuService.DeleteFoodNBev(id);
        }
    }
}
