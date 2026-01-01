// using Vben.Common.Core.Manager;
//
// namespace Vben.Common.Core.Utils;
//
// public class XuserUtil
// {
//     // public static SysOrg getUser()
//     // {
//     //     var user = App.GetService<IUserManager>();
//     //     return new SysOrg(user.UserId, user.Name);
//     // }
//
//     public static String getUserId()
//     {
//         String userId = null;
//         var user = MyApp.GetHttpService<IUserManager>();
//         if (user != null)
//         {
//             userId = user.UserId;
//         }
//
//         if (userId == null)
//         {
//             userId = "u1";
//         }
//         return userId;
//     }
//
// }