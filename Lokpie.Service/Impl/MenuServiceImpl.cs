using AutoMapper;
using Lokpie.Common.Commands;
using Lokpie.Common.Dtos;
using Lokpie.Common.Queries;
using Lokpie.Repository;
using Lokpie.Repository.Models;
using QSI.Common.Web;
using QSI.Persistence.Query;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lokpie.Service.Impl
{
    public class MenuServiceImpl : IMenuService
    {
        private readonly IMapper mapper;
        private readonly ITenantDao tenantDao;
        private readonly IFoodNBevDao foodNBevDao;
        private readonly ConditionFactory conditionFactory;

        public MenuServiceImpl(IMapper mapper, ITenantDao tenantDao, ITenantTableDao tenantTableDao, IFoodNBevDao foodNBevDao, ConditionFactory conditionFactory, IOrderDao orderDao, IOrderDetailDao orderDetailDao, IReservationDao reservationDao, IHotMenuRankDao hotMenuRankDao)
        {
            this.mapper = mapper;
            this.tenantDao = tenantDao;
            this.foodNBevDao = foodNBevDao;
            this.conditionFactory = conditionFactory;
        }

        #region Food n Bev
        public long AddNewFoodNBev(AddFoodNBevCommand command)
        {
            FoodNBev newFoodNBev = mapper.Map<FoodNBev>(command);
            Tenant tenant = tenantDao.Get(command.TenantId);
            if (tenant != null)
            {
                newFoodNBev.Tenant = tenant;
            }

            if (command.Photo != null)
            {
                byte[] imageBytes = Convert.FromBase64String(command.Photo);
                File.WriteAllBytes($"C:\\Publish\\Lokpie_Assets\\{command.FileName}", imageBytes);
                newFoodNBev.Photo = $"C:\\Publish\\Lokpie_Assets\\{command.FileName}";
            }

            newFoodNBev.IsDeleted = false;
            var foodNBev = foodNBevDao.Save(newFoodNBev);

            return foodNBev.Id;
        }

        public long UpdateFoodNBev(UpdateFoodNBevCommand command)
        {
            var foodNBev = foodNBevDao.Get(command.Id);
            if (foodNBev != null)
            {
                foodNBev.IsDeleted = false;
                foodNBev.Description = command.Description;
                foodNBev.Name = command.Name;
                foodNBev.Price = command.Price;
                foodNBev.FileName = command.FileName;
                foodNBev.Stock = command.Stock;
                foodNBev.Type = command.Type;
                foodNBev.Status = command.Status;
                foodNBev.Category = command.Category;
                foodNBev.Discount = command.Discount;
                if (command.Photo != null)
                {
                    if (foodNBev.Photo != null)
                        File.Delete(foodNBev.Photo);
                    byte[] imageBytes = Convert.FromBase64String(command.Photo);
                    File.WriteAllBytes($"C:\\Publish\\Lokpie_Assets\\{command.FileName}", imageBytes);
                    foodNBev.Photo = $"C:\\Publish\\Lokpie_Assets\\{command.FileName}";
                }
                foodNBevDao.Save(foodNBev);
            }

            return command.Id;
        }

        public DataTable<FoodNBevDto> GetAllFoodNBev(FoodNBevQuery query, int pageStartRow, int pageSize)
        {
            ICondition condition = conditionFactory.Create();
            if (query.tenantId != null && query.tenantId != 0)
                condition.Column("Tenant.Id").Equal(query.tenantId.Value);
            if (query.type.HasValue)
                condition.Column("Type").Equal(query.type.Value);
            if (query.category.HasValue)
                condition.Column("Category").Equal(query.category.Value);
            if (query.Status.HasValue)
                condition.Column("Status").Equal(query.Status.Value);
            condition.Column("IsDeleted").Equal(false);
            IList<FoodNBevDto> foodNBevDtos = mapper.Map<List<FoodNBevDto>>(foodNBevDao.GetFoodNBevPaging(condition, (pageStartRow * pageSize), pageSize));
            DataTable<FoodNBevDto> dtFoodNBevDto = new DataTable<FoodNBevDto>()
            {
                Page = pageStartRow,
                TotalPages = Convert.ToInt32(Math.Ceiling((double)foodNBevDao.GetCount(condition) / pageSize)),
                TotalDatas = Convert.ToInt32(Math.Ceiling((double)foodNBevDao.GetCount(condition))),
                Data = foodNBevDtos
            };
            return dtFoodNBevDto;
        }

        public BasePhotoDto GetFoodNBevPhoto(long menuId)
        {
            BasePhotoDto basePhoto = new BasePhotoDto();
            var foodnBev = foodNBevDao.Get(menuId);
            if (foodnBev != null && foodnBev.Photo != null)
            {
                byte[] imageBytes = File.ReadAllBytes(foodnBev.Photo);
                basePhoto.Photo = Convert.ToBase64String(imageBytes);
                basePhoto.FileName = foodnBev.FileName;
            }
            return basePhoto;
        }

        public FoodNBevDetailDto GetFoodNBevDetail(long menuId)
        {
            var foodnBev = foodNBevDao.Get(menuId);
            FoodNBevDetailDto foodNBevDetail = mapper.Map<FoodNBevDetailDto>(foodNBevDao.Get(menuId));
            if (foodnBev != null && foodnBev.Photo != null)
            {
                byte[] imageBytes = File.ReadAllBytes(foodnBev.Photo);
                foodNBevDetail.Photo = Convert.ToBase64String(imageBytes);
            }
            return foodNBevDetail;
        }

        public void DeleteFoodNBev(long id)
        {
            var foodNBev = foodNBevDao.Get(id);
            if (foodNBev != null)
            {
                foodNBev.IsDeleted = true;
                foodNBevDao.Save(foodNBev);
            }
        }

        #endregion
    }
}
