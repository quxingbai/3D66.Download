using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _3D66.Download.Scripts
{
    public class CookieControl
    {
        static CookieControl()
        {
            FileStream f = new FileStream("./DataFile/Cookie.txt",FileMode.Open);
            StreamReader re = new StreamReader(f);
            C_3D66[0] = re.ReadToEnd();
            f.Dispose();
            
        }
        public static int CookiePointer3D66 = 0;
        public static int CookieCount3D66 { get => C_3D66.Length; }
        /// <summary>
        /// 是不是还有溜溜的Cookie
        /// </summary>
        public static bool HasCookie3D66 { get => CookiePointer3D66 < C_3D66.Length-1; }
        private static String[] C_3D66 ={
            "",
            //"eyeofcloudEndUserId=oeu1616574584929r0.1804348146783905; eyeofcloudSegments=%7B%221%22%3A%22gc%22%2C%222%22%3A%22false%22%2C%223%22%3A%22referral%22%7D; gr_user_id=3dc35c73-6b70-4b0a-b673-b40c37e41159; UM_distinctid=178635a99bc92-0f75eab9046d58-5c3f1d4d-1fa400-178635a99bd3b8; user_behavior_id=F14D67941AF6F1C2594E23730BD000F90; Hm_lvt_bh_ud=D4C58F9758C50A9DE2B2A37129831990; login_token=db1a468c910f7ff6517488529a1c89c0; eyeofcloudBuckets=%7B%229%22%3A%221000002846%22%7D; ADHOC_MEMBERSHIP_CLIENT_ID1.0=da315be2-994b-df04-79fd-1fcee5476f01; PHPSESSID=lfclh25hc88gm37ebaobunap53; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1619628238,1619628273,1620304332,1621347286; last_login_type=2; userCookieData=%7B%22is_star%22%3A0%2C%22user_id%22%3A175825788%2C%22user_name%22%3A%22%5Cu77bf%5Cu661f%5Cu767e%22%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22user_img%22%3A%22https%3A%5C%2F%5C%2Fthirdwx.qlogo.cn%5C%2Fmmopen%5C%2FQJumM7PlcMCzB6fcZDy3s1v3SaqkU6VRUibIN86zoiaBZKtusgb0zeWKe5dzLypz0FXqLVHkHPyHOrIVsoqg6VFRvRd8dUtgta%5C%2F132%22%2C%22star_level%22%3A0%2C%22user_lb%22%3A1515%2C%22xing_dian%22%3A0%2C%22zeng_dian%22%3A2117%2C%22xuan_dian%22%3A32%2C%22cash_lb%22%3A0%2C%22vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_next_vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip01.png%22%2C%22vip_next_vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip11.png%22%2C%22star_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fstaricon%5C%2Fstar0.png%22%2C%22is_xuanran_vip%22%3A1%2C%22is_need_phone%22%3A0%2C%22is_vr_vip%22%3A0%2C%22is_soft_vip%22%3A0%2C%22is_sc_vip%22%3A1%2C%22all_vip_end_day%22%3A0%2C%22sc_end_day%22%3A353%2C%22is_one_end%22%3A1%2C%22vr_end_day%22%3A0%2C%22xr_end_day%22%3A4%2C%22soft_end_day%22%3A0%2C%22vip_rank%22%3A0%2C%22reg_time%22%3A1617721809%2C%22is_home_page%22%3A0%7D; 95ea25105d564481_gr_session_id=8662ead9-e00f-43c8-a992-62d12203d958; 95ea25105d564481_gr_session_id_8662ead9-e00f-43c8-a992-62d12203d958=true; CNZZDATA1273332647=306475174-1617713593-https%253A%252F%252Fuser.3d66.com%252F%7C1621346270; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1621347297; last_user_name=%E7%9E%BF%E6%98%9F%E7%99%BE",
            //"eyeofcloudEndUserId=oeu1619633273770r0.5597162222983982; eyeofcloudSegments=%7B%221%22%3A%22gc%22%2C%222%22%3A%22false%22%2C%223%22%3A%22search%22%7D; eyeofcloudBuckets=%7B%7D; user_behavior_id=72112EE4CD4808832E022EEB0D68C2EF0; UM_distinctid=17919aa93b22e6-04f22112ec3dce-d7e1739-1fa400-17919aa93b37c4; PHPSESSID=h19ddqjstrkldn5bv00oppe64d; Hm_lvt_bh_ud=802A27AB00F467F3565B8A07415C57B8; gr_user_id=29a3039a-4f6e-49b7-a019-4531b5008ef5; 95ea25105d564481_gr_session_id=ead134c8-787d-488a-b300-86ab46a61890; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1619633294; 95ea25105d564481_gr_session_id_ead134c8-787d-488a-b300-86ab46a61890=true; login_token=959228ef2ca1f45f774b2ee7a19b1a7d; last_login_type=6; e7cab2c1f2c6c3042270137b8e01650a=8B6B4B7C08EDC373D9608F3B97F30EC1; userCookieData=%7B%22user_id%22%3A175892063%2C%22user_name%22%3A%22%E6%BA%9C%E7%B2%89_454798962%22%2C%22user_img%22%3A%22https%3A%2F%2Fstatic.3d66.com%2Fpublic%2Fimages%2Fcommon%2FdefaultHead.jpg%22%2C%22user_lb%22%3A0%2C%22zeng_dian%22%3A0%2C%22xing_dian%22%3A0%2C%22cash_lb%22%3A0%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22xuan_dian%22%3A0%2C%22star_level%22%3A0%2C%22is_star%22%3A0%2C%22reg_time%22%3A1619633784%7D; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1619633785; CNZZDATA1273332647=378040408-1619629877-https%253A%252F%252Fuser.3d66.com%252F%7C1619629877; last_user_name=%E6%BA%9C%E7%B2%89_454798962",//https://yunduanxin.net/info/12162646607/
            //"eyeofcloudEndUserId=oeu1619633016942r0.201305932688864; eyeofcloudSegments=%7B%221%22%3A%22gc%22%2C%222%22%3A%22false%22%2C%223%22%3A%22search%22%7D; eyeofcloudBuckets=%7B%7D; user_behavior_id=8588418A00035A2CD88157648F734FF80; PHPSESSID=0smgr147sf0ef3qvs0osjkmkp9; UM_distinctid=17919a6b0f619e-01bda2218be491-d7e1739-1fa400-17919a6b0f713a; gr_user_id=1df18e04-3e81-4061-b429-ad2b2291e722; 95ea25105d564481_gr_session_id=aa600e16-fc13-4013-ab66-8a3729fb9f2c; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1619633025; Hm_lvt_bh_ud=3A70691B5EBD7B88B6A20E8CA5138565; 95ea25105d564481_gr_session_id_aa600e16-fc13-4013-ab66-8a3729fb9f2c=true; login_token=5f366a443768dfc2df7d094ce868d70f; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1619633042; last_login_type=5; cf8476946fdb1a84d863028a692b729b=7F091752C1609014E6EB5600753D3111; userCookieData=%7B%22user_id%22%3A175672807%2C%22user_name%22%3A%22%E6%BA%9C%E7%B2%89_916595545%22%2C%22user_img%22%3A%22https%3A%2F%2Fstatic.3d66.com%2Fpublic%2Fimages%2Fcommon%2FdefaultHead.jpg%22%2C%22user_lb%22%3A0%2C%22zeng_dian%22%3A0%2C%22xing_dian%22%3A0%2C%22cash_lb%22%3A0%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22xuan_dian%22%3A2%2C%22star_level%22%3A0%2C%22is_star%22%3A0%2C%22reg_time%22%3A1609830844%7D; CNZZDATA1273332647=1000227338-1619629877-https%253A%252F%252Fuser.3d66.com%252F%7C1619629877",//https://yunduanxin.net/info/12165846716/
            //"eyeofcloudEndUserId=oeu1619617899029r0.030676779773673557; eyeofcloudBuckets=%7B%7D; gr_user_id=8ba0ddc3-4e62-4031-a4ca-1121f8fd488c; 95ea25105d564481_gr_session_id=02840cbb-ce10-4260-ab58-9b5348988430; UM_distinctid=17918bfe52c1d0-003a068682ca67-d7e1739-1fa400-17918bfe52dda1; user_behavior_id=E7B0ABDDE3FEFADCE329F8D34E0BC21E0; PHPSESSID=l0moef565a4ctjulrhiuceq6gd; Hm_lvt_bh_ud=0FEA5818B24E525469EC1839DBA06934; login_token=5ec6fe5675d833764029ae0a0c664107; get_cookie_time=1619661113; eyeofcloudSegments=%7B%221%22%3A%22gc%22%2C%222%22%3A%22false%22%2C%223%22%3A%22referral%22%7D; 95ea25105d564481_gr_session_id_02840cbb-ce10-4260-ab58-9b5348988430=true; CNZZDATA1273332647=1856789038-1619613198-https%253A%252F%252Fwww.3d66.com%252F%7C1619618602; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1619617899,1619617913,1619618963; key_name=; userBindPhone=; last_login_type=6; 8f9b80e103d12fdf9e582bc468739afb=B61A69C2086A74CE1B3EE4F709C97E6D; userCookieData=%7B%22user_id%22%3A175891916%2C%22user_name%22%3A%22%E6%BA%9C%E7%B2%89_610819587%22%2C%22user_img%22%3A%22https%3A%2F%2Fstatic.3d66.com%2Fpublic%2Fimages%2Fcommon%2FdefaultHead.jpg%22%2C%22user_lb%22%3A0%2C%22zeng_dian%22%3A0%2C%22xing_dian%22%3A0%2C%22cash_lb%22%3A0%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22xuan_dian%22%3A0%2C%22star_level%22%3A0%2C%22is_star%22%3A0%2C%22reg_time%22%3A1619623348%7D; last_user_name=%E6%BA%9C%E7%B2%89_610819587; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1619623355",//https://yunduanxin.net/info/8618411631209/
            //"eyeofcloudEndUserId=oeu1619631256812r0.5517592262590867; eyeofcloudSegments=%7B%221%22%3A%22gc%22%2C%222%22%3A%22false%22%2C%223%22%3A%22search%22%7D; eyeofcloudBuckets=%7B%7D; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1619631257; gr_user_id=046bc4bf-18e5-4d5f-9610-e5cc53bd0b26; 95ea25105d564481_gr_session_id=65a185dc-2389-4bd5-98cb-a4c1cf2b3890; user_behavior_id=3BADE23342F9D128B290BA1DA56C54520; PHPSESSID=956heerp9gr8tovsait7ciu2cs; UM_distinctid=179198bb6e2374-00f909d3e8516d-d7e1739-1fa400-179198bb6e311f; 95ea25105d564481_gr_session_id_65a185dc-2389-4bd5-98cb-a4c1cf2b3890=true; Hm_lvt_bh_ud=2D5970FEEC44EA8C85C53310445CB0D1; login_token=5dc2cadbeb0e7af1842e1d1d759af459; last_login_type=5; a3a9bfc8b0cddc95a8d6b7f08d62e148=59DA8DE8FAD8826E81B64199200B9E94; userCookieData=%7B%22user_id%22%3A3331975%2C%22user_name%22%3A%22%E6%BA%9C%E7%B2%89_918032463%22%2C%22user_img%22%3A%22https%3A%2F%2Fstatic.3d66.com%2Fpublic%2Fimages%2Fcommon%2FdefaultHead.jpg%22%2C%22user_lb%22%3A0%2C%22zeng_dian%22%3A0%2C%22xing_dian%22%3A0%2C%22cash_lb%22%3A0%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22xuan_dian%22%3A0%2C%22star_level%22%3A0%2C%22is_star%22%3A0%2C%22reg_time%22%3A1606799651%7D; CNZZDATA1273332647=755910574-1619629877-https%253A%252F%252Fuser.3d66.com%252F%7C1619629877; last_user_name=%E6%BA%9C%E7%B2%89_918032463; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1619631405",//https://yunduanxin.net/info/8618411632868/
        };
        public static String GetCookie()
        {
            return C_3D66[CookiePointer3D66];
        }
        public static bool PointerToNext()
        {
            if (HasCookie3D66)
            {
                CookiePointer3D66 += 1;
                return true;
            }
            return false;
        }
    }
}
