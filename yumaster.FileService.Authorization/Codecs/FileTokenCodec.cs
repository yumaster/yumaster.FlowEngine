using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using yumaster.FileService.Authorization.Options;
using yumaster.FileService.Authorization.Utils;

namespace yumaster.FileService.Authorization.Codecs
{
    /// <summary>
    /// FileToken编解码器
    /// </summary>
    public class FileTokenCodec : IFileTokenCodec
    {
        private readonly byte[] _appSecretBytes;
        public const byte CurrentVersion = 2;
        private readonly IUrlDataCodec _urlDataCodec;

        public FileTokenCodec(IOptions<AuthOption> tokenOpt, IUrlDataCodec urlDataCodec)
        {
            _appSecretBytes = tokenOpt.Value.GetAppSecretBytes();
            _urlDataCodec = urlDataCodec;
        }
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string Encode(FileToken token)
        {
            var pseudoIdBys = NetBitConverter.GetBytes(token.PseudoId);
            var fileIdBys = NetBitConverter.GetBytes(token.FileId);
            var ownerIdBys = NetBitConverter.GetBytes(token.FileOwnerId);
            var mimeBys = NetBitConverter.GetBytes(token.MimeId);
            var expireTimeBys = GetBytes(token.ExpireTime);
            var fileCreateTimeBys = GetBytes(token.FileCreateTime);

            var lstLen = 1 + pseudoIdBys.Length + fileIdBys.Length + ownerIdBys.Length + mimeBys.Length + expireTimeBys.Length + fileCreateTimeBys.Length;
            var mdatLst = new List<byte>(lstLen);
            mdatLst.Add(CurrentVersion);
            mdatLst.AddRange(pseudoIdBys);
            mdatLst.AddRange(fileIdBys);
            mdatLst.AddRange(ownerIdBys);
            mdatLst.AddRange(mimeBys);
            mdatLst.AddRange(expireTimeBys);
            mdatLst.AddRange(fileCreateTimeBys);

            var mdatBys = mdatLst.ToArray();

            //签名
            var signBys = ArrayUtil.Addition(_appSecretBytes, mdatBys);
            var hashBys = Md5(signBys);

            //编码成字符串
            var encBys = ArrayUtil.Addition(hashBys, mdatBys);
            return _urlDataCodec.Encode(encBys);
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public FileToken Decode(string tokenStr)
        {
            var encBys = _urlDataCodec.Decode(tokenStr);

            //校验签名
            var hashLen = 16;
            var mdatBys = new byte[encBys.Length - hashLen];
            Array.Copy(encBys, hashLen, mdatBys, 0, mdatBys.Length);
            var signBys = ArrayUtil.Addition(_appSecretBytes, mdatBys);
            var hashBys = Md5(signBys);
            if (!ArrayUtil.Equals(hashBys, 0, encBys, 0, hashLen))
                throw new InvalidDataException("bad sign");

            if (mdatBys[0] != CurrentVersion)
                throw new NotSupportedException("bad token version");

            //解析成对象
            var index = 1; //忽略版本
            var pseudoId = NetBitConverter.ToUInt32(mdatBys, index);
            index += 4;
            var fileId = NetBitConverter.ToInt32(mdatBys, index);
            index += 4;
            var ownerId = NetBitConverter.ToInt32(mdatBys, index);
            index += 4;
            var mimeId = NetBitConverter.ToUInt32(mdatBys, index);
            index += 4;
            var expireTime = ToDateTime(mdatBys, index);
            index += sizeof(long);
            var fileCreateTime = ToDateTime(mdatBys, index);

            return new FileToken
            {
                PseudoId = pseudoId,
                FileId = fileId,
                FileOwnerId = ownerId,
                MimeId = mimeId,
                ExpireTime = expireTime,
                FileCreateTime = fileCreateTime
            };
        }

        private static byte[] GetBytes(DateTime dt)
        {
            return NetBitConverter.GetBytes(dt.ToBinary());
        }

        private static DateTime ToDateTime(byte[] bytes, int startIndex)
        {
            var l = NetBitConverter.ToInt64(bytes, startIndex);
            return DateTime.FromBinary(l);
        }

        private static byte[] Md5(byte[] bytes)
        {
            return Md5(bytes, 0, bytes.Length);
        }

        private static byte[] Md5(byte[] bytes, int offset, int count)
        {
            using (var hashAlgo = MD5.Create())
            {
                return hashAlgo.ComputeHash(bytes, offset, count);
            }
        }
    }
}
