using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<DocumentController> _logger;
        private readonly IConfiguration _configurationonfiguration;

        public DocumentController(IMapper mapper, IRepositoryWrapper repository, ILogger<DocumentController> logger, IConfiguration configurationonfiguration)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
            _configurationonfiguration = configurationonfiguration;
        }

        [HttpGet]
        [Route("Document/GetDocumentListbyRkey")]
        public ListResult<DocumentDto> GetCatProductById(long rkey)
        {
            try
            {
                var res = _repository.Document.FindByCondition(c => c.CatDocument.Rkey == rkey && c.Ddate == null && c.DaDate == null).Include(c=>c.CatDocument)
                    .ToList();
                var result = _mapper.Map<List<DocumentDto>>(res);

                return ListResult<DocumentDto>.GetSuccessfulResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ListResult<DocumentDto>.GetFailResult(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Document/UploadSellerDocument")]
        public LongResult UploadSellerDocument(long documentId)
        {
            try
            {
               var userId= ClaimPrincipalFactory.GetUserId(User);
                var sellerId = _repository.Seller.FindByCondition(c => c.UserId == userId).Select(c => c.Id).FirstOrDefault();
                var docpath = "";
                var documentfile = HttpContext.Request.Form.Files.GetFile("Document");
                if (documentfile != null)
                {
                    var uploadFileStatus = FileManeger.FileUploader(documentfile, 1, "SellerDocuments");
                    if (uploadFileStatus.Status == 200)
                    {
                        docpath = uploadFileStatus.Path;
                    }
                    else
                    {
                        return LongResult.GetFailResult(uploadFileStatus.Path);
                    }

                }

                SellerDocument doc = new SellerDocument
                {

                    FileUrl = docpath,
                    SellerId = sellerId,
                    CuserId = userId,
                    Cdate = DateTime.Now.Ticks,
                    DocumentId = documentId

                };
                _repository.SellerDocument.Create(doc);
                _repository.Save();

                return LongResult.GetSingleSuccessfulResult(doc.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return LongResult.GetFailResult(e.Message);
            }
        }

    }
}
