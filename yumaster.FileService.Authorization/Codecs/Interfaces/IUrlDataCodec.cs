namespace yumaster.FileService.Authorization.Codecs
{
    /// <summary>
    /// URL承载的数据编解码器
    /// </summary>
    public interface IUrlDataCodec
    {
        string Encode(byte[] data);
        byte[] Decode(string encedStr);
    }
}
