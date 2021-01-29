using JaegerNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static JaegerNetCore.HttpClientHelper;

namespace JaegerNetCore.Controllers
{
    [ApiController]
    [Route("school")]
    public class SchoolController : ControllerBase
    {
        private readonly HttpClient _schoolClient;
        public SchoolController(IHttpClientFactory httpClientFactory)
        {
            _schoolClient = httpClientFactory.CreateClient("openapi");
        }
        /// <summary>
        /// 学校详情
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        [HttpGet("{schoolId}/grade/{gradeId}")]
        public async Task<SchoolViewModel> GetSchoolDetailAsync(int schoolId, int gradeId)
        {
            SchoolViewModel school = new SchoolViewModel();
            var responseMessage = await _schoolClient.GetAsync("/api/school/" + schoolId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var r = await ReadAsObjectAsync<DataResponse<SchoolViewModel>>(responseMessage.Content);
                school = r.Data;
            }

            //var gradeId = new Random().Next(1, 14);
            responseMessage = await _schoolClient.GetAsync($"/api/class/bygradeididandschoolid?schoolId={schoolId}&gradeId={gradeId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var r = await ReadAsObjectAsync<DataResponse<List<ClassViewModel>>>(responseMessage.Content);
                if (r.Data != null)
                    school.Classes.AddRange(r.Data);
            }
            return school;
        }
        /// <summary>
        /// 学校详情，redis
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        [HttpGet("redis/{schoolId}/grade/{gradeId}")]
        public async Task<SchoolViewModel> GetSchoolDetailFromRedisAsync(int schoolId, int gradeId)
        {
            SchoolViewModel school = new SchoolViewModel();
            var cacheKey = "School_" + school + "_Grade_" + gradeId;
            var cacheItem = await RedisHelper.GetAsync<SchoolViewModel>(cacheKey);
            if (cacheItem != null)
                return cacheItem;

            var responseMessage = await _schoolClient.GetAsync("/api/school/" + schoolId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var r = await ReadAsObjectAsync<DataResponse<SchoolViewModel>>(responseMessage.Content);
                school = r.Data;
            }

            //var gradeId = new Random().Next(1, 14);
            responseMessage = await _schoolClient.GetAsync($"/api/class/bygradeididandschoolid?schoolId={schoolId}&gradeId={gradeId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var r = await ReadAsObjectAsync<DataResponse<List<ClassViewModel>>>(responseMessage.Content);
                if (r.Data != null)
                {
                    school.Classes.AddRange(r.Data);
                    await RedisHelper.SetAsync(cacheKey, school);
                }

            }
            return school;
        }


    }
}
