using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using yumaster.FileService.Authorization;
using yumaster.FileService.Authorization.Codecs;
using yumaster.FileService.Service;
using yumaster.FileService.Service.Models.Result;
using yumaster.FileService.WebApi.Models.Input.User;
using yumaster.FileService.WebApi.Options;

namespace yumaster.FileService.WebApi.Controllers
{
    /// <summary>
    /// 内部通信用的接口
    /// </summary>
    [Route("~/api/cluster")]
    public class ClusterController : BaseServerApiController
    {
        private readonly IStorageService _storageSvce;
        private readonly IFileTokenCodec _fileTokenCodec;
        private readonly IOwnerTokenCodec _ownerTokenCodec;

        public ClusterController(IStorageService storageSvce, IFileTokenCodec fileTokenCodec, IOwnerTokenCodec ownerTokenCodec,
            IOptionsMonitor<ManageOption> manageOption) : base(manageOption)
        {
            _storageSvce = storageSvce;
            _fileTokenCodec = fileTokenCodec;
            _ownerTokenCodec = ownerTokenCodec;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        [HttpGet("files/{fileId}/exists")]
        public IActionResult RawFileExists(int fileId, uint pseudoId, DateTime fileCreateTime)
        {
            var exists = _storageSvce.RawFileExists(pseudoId, fileCreateTime, fileId);

            return Json(new Result(exists ? ResultErrorCodes.Success : 100));
        }

        /// <summary>
        /// 服务器间同步文件
        /// </summary>
        [HttpPost("files")]
        public async Task<IActionResult> SyncFileAsync(SyncFileModel model)
        {
            FileToken fToken;
            if (!_fileTokenCodec.TryDecode(model.FileToken, out fToken))
                return Json(new Result(ResultErrorCodes.ArgumentBad, "bad fileToken"));

            var filePath = _storageSvce.GetRawFilePath(fToken.PseudoId, fToken.FileCreateTime, fToken.FileId);
            //await _storageSvce.ReceiveToPathAsync(model.File, filePath);
            throw new NotImplementedException();

            return Json(new Result(ResultErrorCodes.Success));
        }
    }
}