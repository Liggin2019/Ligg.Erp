using Ligg.Abp.Application.Contracts;
using Ligg.Abp.Domain;
using Ligg.Abp.Domain.Configurations;
using Ligg.Abp.Domain.IRepositories;
using Ligg.Abp.Domain.Shared.Enums;
using Ligg.Base.DataModel.Paged;
using Ligg.Base.DataModel.ServiceResult;
using Ligg.Base.Extensions;
using Ligg.Base.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using static Ligg.Abp.Domain.Shared.Consts;

namespace Ligg.Abp.Application.Impl
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IOrganizationRepository _orgRepository;
        private readonly IUserRepository _usrRepository;
        private readonly IRoleRepository _rolRepository;
        public UserService(IOrganizationRepository orgRepository, IRoleRepository rolRepository, IUserRepository usrRepository)
        {
            _orgRepository = orgRepository;
            _rolRepository = rolRepository;
            _usrRepository = usrRepository;
        }


        public async Task<ServiceResultT<PagedList<UserMtnListDto>>> ListUserByDepartment(string deptId, int pageIndex, int pageSize)
        {
            var result = new ServiceResultT<PagedList<UserMtnListDto>>();
            var list = await _usrRepository.GetListAsync();
            var list1 = list.FindAll(x => x.DepartmentId.ToString() == deptId&x.Builtin==false);
            var list2 = ObjectMapper.Map<IEnumerable<User>, IEnumerable<UserMtnListDto>>(list1);
            list2 = list2.OrderByDescending(x => x.ModificationTime);
            var count = list2.ToList().Count;
            var list3 = list2.AsQueryable().PageByIndex(pageIndex, pageSize);
            foreach (var obj in list3)
            {
                var obj1 = list.Find(x => x.Id == obj.Id);
                obj.StateFlag = obj1.Disabled?"禁用":"";
                obj.CreatorCode = (obj1.CreatorId==null) ? "" :GetCodeById(obj1.CreatorId);
                obj.LastModifierCode = (obj1.CreatorId == null) ? "" : GetCodeById(obj1.LastModifierId);
            }
            result.IsSuccess(new PagedList<UserMtnListDto>(count, list3.ToList()));
            return result;
        }
        //public async Task<ServiceResultT<PagedList<UserMtnListDto>>> ListUserByDepartmentOld(string deptId, int pageIndex, int pageSize)
        //{
        //    //var result = new ServiceResultT<PagedList<UserMtnListDto>>();
        //    //var items = await _usrRepository.GetListAsync();

        //    //var list = _usrRepository.Where(x => x.Builtin == false).OrderByDescending(x => x.ModificationTime)
        //    //                      .Select(x => new UserMtnListDto
        //    //                      {
        //    //                          Id = x.Id,
        //    //                          Code = x.Code,
        //    //                          Name = x.Name,
        //    //                          Email = x.Email,
        //    //                          Disabled = x.Disabled,
        //    //                          //DepartmentId = x.DepartmentId,
        //    //                          CreationTime = x.CreationTime,
        //    //                          ModificationTime = x.ModificationTime,

        //    //                      }).Where(x => x.DepartmentId.ToString().ToLower() == deptId.ToLower());
        //    //if (list == null) return null;
        //    //var count = list.ToList().Count;
        //    //var list1 = list.AsQueryable().PageByIndex(pageIndex, pageSize);
        //    //result.IsSuccess(new PagedList<UserMtnListDto>(count, list1.ToList()));
        //    //return result;
        //}

        public async Task<ServiceResultT<PagedList<UserMtnListDto>>> ListUserByRole(string rolId, int pageIndex, int pageSize)
        {
            var result = new ServiceResultT<PagedList<UserMtnListDto>>();
            var items = await _usrRepository.GetListAsync();

            //var list = ObjectMapper.Map<IEnumerable<User>, IEnumerable<UserMtnListDto>>(items);
            //list = list.Where(x => x.DepartmentId.ToString().ToLower() == deptId.ToLower());
            //result.IsSuccess(list);
            //return result;
            return null;


        }

        //#add
        public async Task<ServiceResult> AddUser(UserAddDto input)
        {
            var result = new ServiceResult();
            var sameCodeUser = _usrRepository.Where(x => x.Code == input.Code).FirstOrDefault();
            if (sameCodeUser != null)
            {
                result.IsFailed("User codes can not repeat!");
                return result;
            }

            var org = _orgRepository.GetAsync(x => x.Id == input.DepartmentId);
            if (org == null)
            {
                result.IsFailed("Department does not exsit!");
                return result;
            }
            if (org.Result.Type != (int)OrganizationType.Department)
            {
                result.IsFailed("User can only be created under department!");
                return result;
            }

            var user = new User();
            user.Code = input.Code;
            user.Name = input.Name;
            user.Password = EncryptionHelper.Md5Encrypt(input.Password);
            user.Email = input.Email;
            user.DepartmentId = input.DepartmentId;
            user.Disabled = false;
            user.Builtin = false;
            user.CreatorId = new Guid(CurrentUser.FindClaim("Id").Value);
            user.CreationTime = DateTime.Now;
            user.LastModifierId = new Guid(CurrentUser.FindClaim("Id").Value);
            user.ModificationTime = DateTime.Now;

            await _usrRepository.InsertAsync(user);

            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        //#mod
        public async Task<ServiceResult> ModifyUser(UserModDto input)
        {
            var result = new ServiceResult();
            var user = await _usrRepository.GetAsync(input.Id);
            if (user == null)
            {
                result.IsFailed("User does not exsit!");
                return result;
            }

            user.Code = input.Code;
            user.Name = input.Name;
            user.Email = input.Email;
            user.LastModifierId = new Guid(CurrentUser.FindClaim("Id").Value);
            user.ModificationTime = DateTime.Now;

            await _usrRepository.UpdateAsync(user);

            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        public async Task<ServiceResult> ChangePassword(UserModDto input)
        {
            var result = new ServiceResult();

            var user = await _usrRepository.GetAsync(input.Id);
            if (user == null)
            {
                result.IsFailed("User does not exsit!");
                return result;
            }

            user.Password = EncryptionHelper.Md5Encrypt(input.Password);
            user.LastModifierId = new Guid(CurrentUser.FindClaim("Id").Value);
            user.ModificationTime = DateTime.Now;

            await _usrRepository.UpdateAsync(user);

            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        public async Task<ServiceResult> EnDisableUser(int type, Guid id)
        {
            var result = new ServiceResult();

            var user = await _usrRepository.GetAsync(id);
            if (user == null)
            {
                result.IsFailed("User does not exsit!");
                return result;
            }
            if (type == 0)
            {
                user.Disabled = true;
            }
            else user.Disabled = false;
            user.LastModifierId = new Guid(CurrentUser.FindClaim("Id").Value);
            user.ModificationTime = DateTime.Now;
            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        public async Task<ServiceResult> ChangeDepartment(Guid DeptId, String[] userIds)
        {
            var result = new ServiceResult();

            foreach (var usrId in userIds)
            {
                var user = await _usrRepository.GetAsync(new Guid(usrId));
                if (user == null)
                {
                    result.IsFailed("User does not exsit!");
                    return result;
                }
                user.DepartmentId = DeptId;
                user.LastModifierId = new Guid(CurrentUser.FindClaim("Id").Value);
                user.ModificationTime = DateTime.Now;
            }

            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        //#del
        public async Task<ServiceResult> DeleteUsers(String[] Ids)
        {
            var result = new ServiceResult();
            if (Ids == null | Ids.Length == 0)
            {
                result.IsFailed("Ids can't be null or empty!");
                return result;
            }

            var items = await _usrRepository.GetListAsync();
            var objs = items.Where(x => Ids.Contains(x.Id.ToString()));
            await _usrRepository.BulkRemoveAsync(objs);
            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        //#get
        public async Task<ServiceResultT<UserAddDto>> GetUser(Guid Id)
        {
            var result = new ServiceResultT<UserAddDto>();
            var item = await _usrRepository.GetAsync(Id);

            var obj = ObjectMapper.Map<User, UserAddDto>(item);
            obj.Password = "";
            result.IsSuccess(obj);
            return result;
        }

        public async Task<UserDto> GetUserByCode(string code)
        {

            var item = await _usrRepository.GetAsync(x => x.Code == code);

            var obj = ObjectMapper.Map<User, UserDto>(item);
            return obj;
        }



        //#other
        public async Task<ServiceResult> Logon(string userCode, string userPass)
        {
            var result = new ServiceResult();

            if (string.IsNullOrEmpty(userCode))
            {
                result.IsFailed("userCode不能为空");
                return result;
            }

            if (string.IsNullOrEmpty(userPass))
            {
                result.IsFailed("userPass不能为空");
                return result;
            }


            var usr = _usrRepository.Where(x => x.Code == userCode & x.Disabled == false).FirstOrDefault();
            if (usr == null)
            {
                result.IsFailed("用户不存在，或密码错误！");
                return result;
            }

            var pw = EncryptionHelper.Md5Encrypt(userPass);
            var isPwOk = pw == usr.Password;
            if (!isPwOk)
            {
                result.IsFailed("用户不存在，或密码错误！");
                return result;
            }


            var claims = new[] {
                new Claim("Id", usr.Id.ToString(), ClaimValueTypes.Sid),
                    new Claim(ClaimTypes.Name, userCode),
                    new Claim(ClaimTypes.Email, usr.Email),
                    new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(AppSettings.JWT.Expires)).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
                };

            var key = new SymmetricSecurityKey(AppSettings.JWT.SecurityKey.SerializeUtf8());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: AppSettings.JWT.Domain,
                audience: AppSettings.JWT.Domain,
                claims: claims,
                expires: DateTime.Now.AddMinutes(AppSettings.JWT.Expires),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            result.IsSuccess(token);
            return result;

        }

        //##common   
        private String GetCodeById(Guid? id)
        {

            var obj = _usrRepository.Where(x => x.Id == id).FirstOrDefault();
            if (obj == null) return "";
            return obj.Code;
        }




}
}
