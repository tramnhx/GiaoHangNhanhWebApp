using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Genders;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;

namespace GiaoHangNhanh.Services.Catalog
{
    public interface IGenderService
    {
        Task<ApiResult<List<GenderDto>>> GetAll(ManageGenderPagingRequest request);
        Task<PagedResult<GenderDto>> GetManageListPaging(ManageGenderPagingRequest request);
        Task<ApiResult<int>> CreateAsync(GenderRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<GenderDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(GenderRequest request);
    }
    public class GenderService : IGenderService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public GenderService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<List<GenderDto>>> GetAll(ManageGenderPagingRequest request)
        {
            var query = from p in _context.Genders
                        select new { p };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.p.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<GenderDto>>(await query.Select(x => new GenderDto()
            {
                Id = x.p.Id,
                Code = x.p.Code,
                Name = x.p.Name
            }).AsNoTracking().ToListAsync());
        }
        public async Task<ApiResult<int>> CreateAsync(GenderRequest request)
        {
            try
            {
                var gender = new Gender()
                {
                    Code = request.Code,
                    Name = request.Name
                };
               
                _context.Genders.Add(gender);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(gender.Id);
            }
            catch(Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }            
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            try
            {
                var genders = await _context.Genders.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (genders == null) throw new ApplicationException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");

                _context.Genders.RemoveRange(genders);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<PagedResult<GenderDto>> GetManageListPaging(ManageGenderPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Genders
                        select new { p };
            
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.p.Name.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new GenderDto()
            {
                Id = x.p.Id,
                Code = x.p.Code,
                Name = x.p.Name,
                CreatedDate = x.p.CreatedDate
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<GenderDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<GenderDto>> GetById(int id)
        {
            var gender = await _context.Genders.FindAsync(id);
          
            var genderDto = new GenderDto()
            {
                Id = gender.Id,
                CreatedDate = gender.CreatedDate,
                Code = gender.Code,
                Name = gender.Name
            };

            return new ApiSuccessResult<GenderDto>(genderDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(GenderRequest request)
        {
            try
            {
                var gender = await _context.Genders.FindAsync(request.Id);
               
                if (gender == null) throw new ApplicationException($"Không tìm thấy Giới tính có id: {request.Id}");

                gender.Code = request.Code;
                gender.Name = request.Name;
                _context.Genders.Update(gender);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
            
        }
    }
}
