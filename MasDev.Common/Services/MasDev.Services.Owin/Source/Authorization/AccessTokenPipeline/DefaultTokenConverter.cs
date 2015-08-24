using System;
using Newtonsoft.Json;
using System.Text;
using MasDev.Services.Modeling;
using MasDev.Utils;
using System.Linq;
using MasDev.Exceptions;

namespace MasDev.Services.Auth
{
    public class DefaultAccessTokenConverter : IAccessTokenConverter
    {
        public string Serialize(IAccessToken token)
        {
            return JsonConvert.SerializeObject(token);
        }

        public IAccessToken Deserialize(string serializedToken)
        {
            return JsonConvert.DeserializeObject<AccessToken>(serializedToken);
        }
    }

    public class CompactAccessTokenConverter : IAccessTokenConverter
    {
        enum Stage { IdentityFlag, IdentityId, IdentityRoles, CreationUtc, ExpirationUtc, Scope, Extra }

        static readonly Stage[] _stages = Enum.GetValues(typeof(Stage)).OfType<Stage>().ToArray();

        IAccessToken IAccessTokenConverter.Deserialize(string serializedToken)
        {
            var token = new AccessToken();
            var stage = 0;

            var builder = new StringBuilder();
            for (var i = 0; i < serializedToken.Length; i++)
            {
                var current = serializedToken[i];
                if (current == ',')
                {
                    ParseStage(builder.ToString(), _stages[stage++], token);
                    builder.Clear();
                }
                else builder.Append(current);
            }

            return token;
        }

        static void ParseStage(string stageContent, Stage stage, IAccessToken token)
        {
            switch (stage)
            {
                case Stage.CreationUtc:
                    token.CreationUtc = DateTime.Parse(stageContent);
                    break;
                case Stage.ExpirationUtc:
                    token.ExpirationUtc = DateTime.Parse(stageContent);
                    break;
                case Stage.Extra:
                    token.Extra = ParseOptionalInteger(stageContent);
                    break;
                case Stage.IdentityFlag:
                    token.Identity = new Identity();
                    token.Identity.Flag = int.Parse(stageContent);
                    break;
                case Stage.IdentityId:
                    token.Identity.Id = int.Parse(stageContent);
                    break;
                case Stage.IdentityRoles:
                    token.Identity.Roles = int.Parse(stageContent);
                    break;
                case Stage.Scope:
                    token.Scope = ParseOptionalInteger(stageContent);
                    break;
                default: throw new ShouldNeverHappenException("Enum not mapped");
            }
        }

        static int? ParseOptionalInteger(string s)
        {
            if (s == StringUtils.Space)
                return null;
            return int.Parse(s);
        }

        public string Serialize(IAccessToken token)
        {
            var identity = token.Identity;
            var builder = new StringBuilder();

            builder.Append(identity.Flag);
            builder.Append(StringUtils.Comma);

            builder.Append(identity.Id);
            builder.Append(StringUtils.Comma);

            builder.Append(identity.Roles);
            builder.Append(StringUtils.Comma);

            builder.Append(token.CreationUtc);
            builder.Append(StringUtils.Comma);

            builder.Append(token.ExpirationUtc);
            builder.Append(StringUtils.Comma);

            builder.Append(token.Extra.HasValue ? token.Extra.ToString() : StringUtils.Space);
            builder.Append(StringUtils.Comma);

            builder.Append(token.Scope.HasValue ? token.Scope.ToString() : StringUtils.Space);
            builder.Append(StringUtils.Comma);

            return builder.ToString();
        }
    }
}

