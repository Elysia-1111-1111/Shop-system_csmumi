using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Shop_System_csmumi_v._0._1._0
{
    public static class LoginSystem
    {
        // Path.Combine 可以自動幫你結合目前的執行檔目錄，並生出一個 user_db.json 的路徑字串
        private static readonly string FilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_db.json");
        // 宣告一個靜態字典，用來在程式執行期間，把從 JSON 解密出來的帳密暫存在記憶體裡
        // Dictionary<string, string> 代表它的結構是： [鍵 (帳號的字串) , 值 (SHA256加密密碼的字串)]
        private static Dictionary<string, string> UserDatabase = new Dictionary<string, string>();
            static LoginSystem()// 這是靜態建構子，名字必須跟類別完全一樣，且不能有 public 或 private
                                // 它的特性是：當程式第一次存取這個類別時，會「自動且強制」執行這裡一次
            {
                LoadDatabaseFromFile();
            }
    //以下為讀取工作人員檔案
            private static void LoadDatabaseFromFile()
            {
            // 1. 如果「第一次執行」，檔案不存在，就直接 return 結束，準備迎接第一個使用者！
                if (!File.Exists(FilePath))
                {
                    UserDatabase.Clear(); // 清空一下字典

                    // 塞入第一組預設的管理員帳密（admin / admin）
                    UserDatabase.Add("admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918");

                    //  呼叫存檔方法，讓它在硬碟裡自動生出第一個 user_db.json
                    SaveDatabaseToFile();
                    return; // 結束讀檔方法
            }
                string jsonText = File.ReadAllText(FilePath);//從地址(FilePath)讀取全部內容並轉存到jsonText
                byte[] base64Bytes = Convert.FromBase64String(jsonText);//把讀回來的資還原回byte陣列(Name:base64Bytes)(後續操作需要)
                string jsonContent = Encoding.UTF8.GetString(base64Bytes);//把陣列base64Bytes根據UTF8編碼的純文字格式
                // 將 JSON 文字還原（反序列化）為帳密字典；若檔案損毀或為空（null），則新建一本空字典，防止程式崩潰。
                UserDatabase = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent) ?? new Dictionary<string, string>();
            }
    //以下儲存工作人員檔案        
            public static void SaveDatabaseToFile()
            {
            try { 
            StringBuilder stringBuilder_02 = new StringBuilder();

            stringBuilder_02.AppendLine("{");//把括號裡的文字貼上去，並且在最後面換行

            List<string> rows = new List<string>();

            foreach (var kvp in UserDatabase)

            {

                //遍歷該檔案並且製成一組一列的檔案放置於動態字串陣列中 "admin": "8c6976..."

                rows.Add($" \"{kvp.Key}\": \"{kvp.Value}\"");

            }

            //把讀出來的陣列 塞進魔術大空間 並且區隔逗號(符合json語法)

            stringBuilder_02.AppendLine(string.Join(",\n", rows));

            //包上結尾的括號

            stringBuilder_02.AppendLine("}");

            //轉換為byte陣列 再轉成 base64 加密字串

            byte[] EncryptionString = Encoding.UTF8.GetBytes(stringBuilder_02.ToString());

            string encryptedContent = Convert.ToBase64String(EncryptionString);

            //把這串密文寫入硬碟檔案中

            System.IO.File.WriteAllText(FilePath, encryptedContent, Encoding.UTF8);

        }
            catch(Exception ex)
            {
                // 萬一寫入硬碟失敗，彈窗警告
                System.Windows.Forms.MessageBox.Show("寫入文檔出錯" + ex.Message);
            }
        }
    //加密過程並回傳加密結果 
        public static string LoginPasswords(string myPassword)
            {
                if (string.IsNullOrEmpty(myPassword)) return string.Empty;
                /*if (...)：如果括號內的條件成立，就執行後面的動作。
                string.IsNullOrEmpty(...)：這是 C# 內建非常實用的工具，用來檢查裡面的字串（這裡指 myPassword）是不是以下兩種情況之一：
                Null：完全沒有記憶體空間（例如：變數還沒初始化、完全是空的）。                          
                Empty：有記憶體空間，但裡面沒有字元（例如：""，使用者按了框框但什麼都沒打）。
                return：代表立刻「跳出」現在這個方法，後面的程式碼（比如後面複雜的加密演算法）通通不執行了。
                string.Empty：就是個精準版的空字串 ""
                */
                using (SHA256 sHA256 = SHA256.Create())//「宣告一個 SHA256 類型的變數叫 sHA256，並請 SHA256 類別工廠幫我派一個做好的實體裝進去。」
                {
                    byte[] arr01 = Encoding.UTF8.GetBytes(myPassword);
                    /*byte[] arr01
                       byte：
                        C# 的基本資料型態，代表一個 8 位元（8-bit）的整數，範圍是 0 ~ 255。
                       []：
                        代表這是一個「陣列（Array）」，也就是一連串、排排站的數字。
                       arr01：
                        變數名稱。合起來就是宣告一個用來裝「一堆 0 ~ 255 數字」的容器。
                       Encoding.UTF8:
                        .NET 內建處理文字編碼的工具。UTF-8 是目前全球網路最通用的編碼標準，它規定了每一個文字（例如 'A'、'1' 或 '中'）對應到電腦底層該用哪些數字來代表。
                       .GetBytes(myPassword):
                            呼叫轉換方法。它會把你的明文密碼（假設是 "123"）根據 UTF-8 的規則轉換成對應的數字。
                    💡      例如：字串 "123" 經過這行處理後，bytes 陣列裡面就會裝著 [49, 50, 51]
                    */
                    byte[] arr01_f = sHA256.ComputeHash(arr01);
                    /*
                    *byte[] arr01_f
                            再次宣告一個新的 Byte 陣列，名稱叫做arr01_f ，用來準備接收加密完後的結果。
                        sHA256
                            就是呼叫那個做好的工具箱(Name:sHA256)。
                        .ComputeHash(bytes)
                            ComputeHash 的字面意思就是「計算雜湊值」。
                            它把剛才換好的 [49, 50, 51] 數字陣列倒進 SHA256 的數學演算法公式裡。
                            SHA256 演算法會像果汁機一樣把這些數字瘋狂攪碎、重新排列組合，固定吐回長度為 32 個 Byte（256 位元）的全新亂數陣列。
                    */
                    StringBuilder stringBuilder = new StringBuilder();// 準備一個高效能的文字拼接白板
                    foreach (byte b in arr01_f)// 逐一取出加密後的每個數字
                    {
                        stringBuilder.Append(b.ToString("x2"));// 將數字轉為 2 位數的十六進位小寫（如：10 轉成 0a）
                    }
                    return stringBuilder.ToString();// 拼湊完成，打包成最終字串傳回
                }
        }
        //驗證正確接口
        public static bool VerifyLogin(string username, string password)
        {
            //如果沒輸入密碼或者帳號回傳false
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;
            //檢查字典內部是否有這筆資料
            /*
            * 在 C# 裡，!（驚嘆號） 代表 「Not（非 / 反轉）」。它會把後面的布林值結果完全顛倒過來：
                !true 會變成 false
                !false 會變成 true
            */
            if(!UserDatabase.ContainsKey(username)) return false;
            //UserDatabase[username]指username所指向的value類似於指標的內容
            string inputHash = LoginPasswords(password);
            return UserDatabase[username] == inputHash;
        }
    
//修改密碼接口
    public static bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            // 1. 先呼叫上面的 VerifyLogin，確認他是本人且知道舊密碼
            if (!VerifyLogin(username, oldPassword)) return false;

            // 2. 本人確認無誤，將他的新密碼加密
            string newHash = LoginPasswords(newPassword);

            // 3. 覆寫記憶體字典裡的舊密碼
            UserDatabase[username] = newHash;

            // 4. 關鍵：立刻重新執行存檔，把新密碼穿上 base64 迷彩寫進硬碟！
            SaveDatabaseToFile();
            return true;
        }
    }
}
/*
* 加密整體流程
*   1.SHA256
*      翻譯成utf8成為byte 加密成為無法復原的64進為亂碼
*   2.站存至dictionary
*      狀態:活物件(UserDatebase)
*      動作:(帳號,SHA256密文)當成成對數據塞進記憶體字典
*   3.轉為json
*      狀態:.json格式文件
*      動作:利用迴圈遍歷整個字典並組合成符合規範的純文字
*   4.穿上base64迷彩
*      狀態:.JSON->base64密文
*      動作:把原本的byte數字陣列轉化為無法看懂的密文
*   5.存檔
*       字串->硬碟檔案
*       呼叫存檔功能 指定地址 編碼 復寫原檔案
*   ___________________________________________________
*   第二階段：回程（啟動程式 ➡️ 讀檔解密 ➡️ 記憶體復活）
1. 驚醒與防禦讀取（ReadAllText）
狀態：硬碟檔案 ➡️ 記憶體密文字串。
動作：程式觸發靜態建構子，自動呼叫 LoadDatabaseFromFile()。先用 File.Exists 檢查有沒有舊檔案（沒有就原地生一個管理員）。有的話，用 File.ReadAllText 把那串沉睡的 Base64 密文字串抓進記憶體。
2. 脫掉迷彩服（還原 byte 數字）
狀態：Base64 密文字串 ➡️ byte[] 數字陣列。
動作：呼叫 Convert.FromBase64String(jsonText)。把這串看起來像亂碼的字串，還原回原本去程時的 0 ~ 255 的底層數字陣列（byte[]）。
3. 翻字典復原（第二次換匯成文字）
狀態：byte[] 數字陣列 ➡️ 純文字 JSON。
動作：呼叫 Encoding.UTF8.GetString(base64Bytes)。翻開 UTF-8 字典，把這堆死沉沉的數字重新翻譯回人類看得懂的 JSON 純文字："{ "admin": "..." }"。
4. 萬能還原機復活（反序列化與雙問號防禦）
狀態：JSON 純文字 ➡️ C# 活字典（UserDatabase）。
動作：呼叫 JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent)，指定好字典形狀，把純文字靈魂復活回 UserDatabase 物件。後面加上 ?? new Dictionary... 作為檔案壞掉時的原地重組防護罩。
5. 現場驗證（登入比對）
狀態：記憶體資料庫就緒，等待使用者輸入。
動作：當使用者在介面輸入帳密按登入，程式觸發 VerifyLogin。把使用者這次輸入的密碼同樣丟進 SHA256 攪碎，然後去剛剛復活的 UserDatabase 字典裡一秒查表對比。如果兩邊的雜湊亂碼完全相同 ➡️ 🎉 登入成功！
*/





