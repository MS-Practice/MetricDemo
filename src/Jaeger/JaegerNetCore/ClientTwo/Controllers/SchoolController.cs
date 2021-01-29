using ClientTwo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static ClientTwo.HttpClientHelper;

namespace ClientTwo.Controllers
{
    [ApiController]
    [Route("api/school")]
    public class SchoolController:ControllerBase
    {
        private readonly HttpClient _schoolClient;
        public SchoolController(IHttpClientFactory httpClientFactory)
        {
            _schoolClient = httpClientFactory.CreateClient("openapi");
        }
        /// <summary>
        /// 获取学校详情
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        [HttpGet("{schoolId}")]
        public async Task<DataResponse<SchoolViewModel>> GetSchoolAsync(int schoolId)
        {
            var responseMessage = await _schoolClient.GetAsync("/api/school/" + schoolId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var r = await ReadAsObjectAsync<DataResponse<SchoolViewModel>>(responseMessage.Content);
                return r;
            }
            return new DataResponse<SchoolViewModel>() { Success = true, Message = "" };
        }
        /// <summary>
        /// 根据学校，年级获取班级信息
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        [HttpGet("{schoolId}/grade/{gradeId}")]
        public async Task<DataResponse<List<ClassViewModel>>> GetSchoolClassesAsync(int schoolId,int gradeId)
        {
            var responseMessage = await _schoolClient.GetAsync($"/api/class/bygradeididandschoolid?schoolId={schoolId}&gradeId={gradeId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var r = await ReadAsObjectAsync<DataResponse<List<ClassViewModel>>>(responseMessage.Content);
                return r;
            }
            return new DataResponse<List<ClassViewModel>>() { Success = true, Message = "" };
        }
    }
}
