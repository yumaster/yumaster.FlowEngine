namespace yumaster.FileService.Authorization.Codecs
{
    /// <summary>
    /// OwnerToken编解码器
    /// </summary>
    public interface IOwnerTokenCodec
    {
        string Encode(OwnerToken token);
        OwnerToken Decode(string tokenStr);
    }

    public static class OwnerTokenCodecExtensions
    {
        public static bool TryDecode(this IOwnerTokenCodec codec, string tokenStr, out OwnerToken token)
        {
            try
            {
                token = codec.Decode(tokenStr);
                return true;
            }
            catch
            {
                token = null;
                return false;
            }
        }
    }
}
