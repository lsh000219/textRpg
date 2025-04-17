using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _04150908
{
    internal class Program
    {
        [Serializable]
        public class Character
        {
            private int level, def, max_hp, hp, gold, a = 0, d = 0, armor = -1, weapon = -1, exp = 0, inven_length=0;
            private string name, chad;
            private float atk;

            private item[] inventory;

            public Character()
            {
                this.level = 01; this.atk = 10; this.def = 5; this.max_hp = 100; this.hp = this.max_hp; this.gold = 15000;
                this.chad = "전사";
                this.inventory = new item[10];
            }

            public int Gold() { return this.gold; }

            public void lose_Gold(int i) { this.gold -= i; }

            public void plus_Exp() { exp += 1; if(exp == level) { this.levelUp(); exp = 0; } }

            private void levelUp() { level += 1; atk += 0.5f; def += 1; }

            private void calculate_Item()
            {
                this.a = 0; this.d = 0;

                for (int i = 0; i < inven_length; i++)
                {
                    if (this.inventory[i].isEquipped() == true)
                    {
                        if (this.inventory[i].getEffect_s() == "공격력") { this.a += this.inventory[i].getEffect_i(); }
                        else if (this.inventory[i].getEffect_s() == "방어력") { this.d += this.inventory[i].getEffect_i(); }
                    }
                }
            }

            public void stat()
            {
                int i = 0, final_def;
                float final_atk = 0;


                while (true)
                {
                    Console.Clear(); calculate_Item();
                    Console.WriteLine("상태 보기\n캐릭터의 정보가 표시됩니다.\n");
                    if(this.a != 0) { final_atk = this.atk + a; Console.WriteLine($"LV. {this.level}\nChad ( {this.chad} )\n공격력 : {final_atk} (+{this.a})"); }
                    else { Console.WriteLine($"LV. {this.level}\nChad ( {this.chad} )\n공격력 : {this.atk}"); }

                    if (this.d != 0) { final_def = this.def + d; Console.WriteLine($"방어력 : {final_def} (+{this.d})\n체 력 : {this.hp}\nGold : {this.gold}\n"); }
                    else { Console.WriteLine($"방어력 : {this.def}\n체 력 : {this.hp}\nGold : {this.gold}\n"); }
                    
                    Console.WriteLine("0. 나가기\n");


                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    if (i != 0) Console.WriteLine("잘못된 입력입니다");
                    i = int.Parse(Console.ReadLine());
                    if (i == 0) break;
                }
            }

            public void inven()
            {
                int i = 0;

                while (true)
                {

                    Console.Clear();
                    Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");
                    Console.WriteLine("[아이템 목록]\n");

                    //아이템 목록 띄우기
                    if(inven_length > 0) { for (int j = 0; j < inven_length; j++) { Console.Write("- "); inventory[j].item_Inven(); } }
                    else { Console.WriteLine("인벤토리에 있는 아이템이 없습니다.\n"); }


                    Console.WriteLine("1. 장착 관리\n0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    if (i != 0 && i != 1) Console.WriteLine("잘못된 입력입니다");

                    i = int.Parse(Console.ReadLine());
                    if (i == 0) break;
                    else if (i == 1) { this.manage_Item();}
                }
            }

            public void get(item item) { inventory[inven_length] = item; inven_length++; }

            private void manage_Item()
            {
                int i = 0, j = 1;


                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("인벤토리 - 장착 관리\n보유 중인 아이템을 관리할 수 있습니다.\n");
                    Console.WriteLine("[아이템 목록]\n");

                    //아이템 목록 띄우기
                    if (inven_length > 0) { for (int k = 0; k < inven_length; k++) { Console.Write($"- {j} "); inventory[k].item_Inven(); j++; } }
                    else { Console.WriteLine("인벤토리에 있는 아이템이 없습니다.\n"); }

                    Console.WriteLine("0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    if (i == -1) Console.WriteLine("잘못된 입력입니다");
                    i = int.Parse(Console.ReadLine());

                    if (i == 0) break;
                    else if (i <= inven_length+1)
                    {
                        if (inventory[i-1].getTag() == "방어구") { 
                            if(armor == -1) { inventory[i - 1].equip_Item(); armor = i - 1; }
                            else
                            {
                                inventory[armor].equip_Item();
                                inventory[i - 1].equip_Item();
                                armor = i - 1;
                            }
                        }
                        else if(inventory[i - 1].getTag() == "무기")
                        {
                            if (weapon == -1) { inventory[i - 1].equip_Item(); weapon = i - 1; }
                            else
                            {
                                inventory[weapon].equip_Item();
                                inventory[i - 1].equip_Item();
                                weapon = i - 1;
                            }
                        }
                        SaveLoad.Save(player, "player.dat");
                    }
                    else i = -1;



                    j = 1;
                }
            }

            public void sell_Item()
            {
                int i = 0, j = 1;


                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("상점 - 아이템 판매\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
                    Console.WriteLine($"[보유 골드]\n{this.gold} G\n");

                    //아이템 목록 띄우기
                    if (inven_length > 0) { for (int k = 0; k < inven_length; k++) { Console.Write($"- {j} "); inventory[k].item_Inven(); j++; } }

                    else { Console.WriteLine("인벤토리에 있는 아이템이 없습니다.\n"); }

                    Console.WriteLine("0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    if (i == -1) Console.WriteLine("잘못된 입력입니다");
                    i = int.Parse(Console.ReadLine());

                    if (i == 0) break;
                    else if (i <= inven_length+1) { this.gold += inventory[i - 1].sell(); this.delete_Item(i-1); SaveLoad.Save(player, "player.dat");
                        SaveLoad.SaveShop(shop, "shop.dat");
                    }
                    else i = -1;



                    j = 1;
                }
            }

            private void delete_Item(int i)
            {
                for (int j = 0; j < inven_length; j++)
                {
                    if(j >= i)
                    {
                        if(inventory[j + 1] != null) { inventory[j] = inventory[j + 1]; }
                        else { inventory[j + 1] = null; }
                    }
                }
                inven_length--;
            }

            public void dungeon()
            {
                int i = 0;

                while (true)
                {

                    Console.Clear();
                    Console.WriteLine("던전입장\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                    Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장\n2. 일반 던전     | 방어력 11 이상 권장");
                    Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장\n0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    if (i == -1) Console.WriteLine("잘못된 입력입니다");

                    i = int.Parse(Console.ReadLine());
                    if (i == 0) break;
                    else if (i == 1) { this.enter_the_dungeon(5); }
                    else if (i == 2) { this.enter_the_dungeon(11); }
                    else if (i == 3) { this.enter_the_dungeon(17); }
                    else { i = -1; }
                }
            }

            private void enter_the_dungeon(int def)
            {
                if ((this.def+this.d) >= def) { this.dungeon_Clear(def); }
                else
                {
                    int chance = new Random().Next(0,10);
                    if (chance <= 3) { this.dungeon_Fail(); }
                    else { this.dungeon_Clear(def); }
                }
            }

            private void dungeon_Clear(int def)
            {
                int i = 0, temp_hp = hp, temp_Gold = gold, blocked = (this.def + this.d) - def;

                int hp_down = new Random().Next(20 - blocked, 35 - blocked);
                hp = hp - hp_down;
                int chance = new Random().Next((int)atk, (int)atk*2+1);

                if (def == 5) { gold += (1000/100)*(100+chance); }
                else if (def == 11) { gold += (1700 / 100) * (100 + chance); }
                else if (def == 17) { gold += (2500 / 100) * (100 + chance); }

                this.plus_Exp();

                SaveLoad.Save(player, "player.dat");

                while (true)
                {

                    Console.Clear();
                    if (def == 5) { Console.WriteLine("던전 클리어\n축하합니다!!\n쉬운 던전을 클리어 하였습니다.\n"); }
                    else if (def == 11) { Console.WriteLine("던전 클리어\n축하합니다!!\n일반 던전을 클리어 하였습니다.\n"); }
                    else if (def == 17) { Console.WriteLine("던전 클리어\n축하합니다!!\n어려운 던전을 클리어 하였습니다.\n"); }

                    Console.WriteLine($"[탐험 결과]\n 체력 {temp_hp} -> {hp}\nGold {temp_Gold} G -> {gold} G\n");
                    Console.WriteLine("0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    if (i == -1) Console.WriteLine("잘못된 입력입니다");

                    i = int.Parse(Console.ReadLine());
                    if (i == 0) break;
                    else { i = -1; }
                }
            }

            private  void dungeon_Fail()
            {
                int i = 0, temp = hp;
                hp = hp / 2;
                SaveLoad.Save(player, "player.dat");

                while (true)
                {

                    Console.Clear();
                    Console.WriteLine("던전 실패\n아쉽습니다...\n던전을 클리어 하지 못했습니다.\n");
                    Console.WriteLine($"[탐험 결과]\n 체력 {temp} -> {hp}\n");
                    Console.WriteLine("0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    if (i == -1) Console.WriteLine("잘못된 입력입니다");

                    i = int.Parse(Console.ReadLine());
                    if (i == 0) break;
                    else { i = -1; }
                }
            }

            public void restScreen()
            {
                int i = 0;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"휴식하기\n500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {this.gold} G | 현재 체력 : {this.hp})\n");
                    Console.WriteLine("1. 휴식하기\n0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    if (i == -1) { Console.WriteLine("잘못된 입력입니다."); }
                    else if (i == -2) { Console.WriteLine("휴식을 완료했습니다."); }
                    else if (i == -3) { Console.WriteLine("Gold 가 부족합니다."); }
                    i = int.Parse(Console.ReadLine());

                    if (i == 0) break;
                    else if(i == 1)
                    {
                        if(this.gold >= 500)
                        {
                            this.hp += 100;
                            if (this.hp > max_hp) { this.hp = max_hp; }
                            this.gold -= 500;
                            i = -2;
                            SaveLoad.Save(player, "player.dat");
                        }
                        else { i = -3; }
                    }
                    else i = -1;
                }
            }
        }


        [Serializable]
        public class item
        {
            private string name, text, effect_s, tag;
            private bool equip = false, bought = false;
            private int price, effect_i;

            public item(string name, string text, string tag, int price, string effect_s, int effect_i)
            {
                this.name = name;
                this.text = text;
                this.tag = tag;
                this.price = price;
                this.effect_s = effect_s;
                this.effect_i = effect_i;
            }

            public void equip_Item() { if (this.equip == false) { this.equip = true; } else { this.equip = false; } }

            public void item_Inven()
            {
                if (equip)
                {
                    Console.WriteLine($"[E]{this.name}   | {effect_s} +{effect_i} | {this.text}");
                }

                else Console.WriteLine($"{this.name}   | {effect_s} +{effect_i} | {this.text}");

            }

            public void item_Sell()
            {
                int i = this.price * 85 / 100;
                if (equip)
                {
                    Console.WriteLine($"[E]{this.name}   | {effect_s} +{effect_i} | {this.text} | {i} G");
                }

                else Console.WriteLine($"{this.name}   | {effect_s} +{effect_i} | {this.text} | {i} G");
            }

            public void item_Shop()
            {
                if(this.bought == false)
                {
                    Console.WriteLine($"{this.name}   | {effect_s} +{effect_i} | {this.text} | {this.price} G");
                }
                else { Console.WriteLine($"{this.name}   | {effect_s} +{effect_i} | {this.text} | 구매완료"); }
            }
            
            public void buy() { this.bought = true; }

            public int sell() {  this.bought = false; return this.price*85/100; }
            
            public int getPrice() { return price; }

            public string getTag() { return tag; }

            public bool isEquipped() { return equip; }

            public bool getBought() {  return bought; }

            public string getEffect_s() { return effect_s; }

            public int getEffect_i() { return effect_i; }
        }


        [Serializable]
        public class Shop
        {
            private item[] stock;
            private int stock_length = 0;

            public Shop()
            {
                this.stock = new item[shop_length];

                item put_item = new item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", "방어구", 1000, "방어력", 5);
                put(put_item);
                put_item = new item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", "방어구", 3500, "방어력", 15);
                put(put_item);
                put_item = new item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", "무기", 1500, "공격력", 5);
                put(put_item);
                put_item = new item("돌멩이", "길바닥에서 주운 돌멩이", "무기", 1, "공격력", 1);
                put(put_item);
            }

            public void put(item item) { stock[stock_length] = item; stock_length++; }

            public void shopping()
            {
                int i = 0, gold;
                while (true)
                {
                    Console.Clear();
                    gold = player.Gold();
                    Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
                    Console.WriteLine($"[보유 골드]\n{gold} G\n");

                    Console.WriteLine($"[아이템 목록]");
                    if (stock_length != 0) { for (int j = 0; j < stock_length; j++) { Console.Write("- "); stock[j].item_Shop(); } }
                    Console.WriteLine("1. 아이템 구매\n2. 아이템 판매 \n0. 나가기\n");


                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    if (i == -1) Console.WriteLine("잘못된 입력입니다");
                    i = int.Parse(Console.ReadLine());

                    if (i == 0) break;
                    else if (i == 1) { this.buying_Item(); }
                    else if (i == 2) { player.sell_Item(); }
                    else { i = -1; }

                }

            }

            private void buying_Item()
            {

                int i = 0, j = 1, gold;
                while (true)
                {
                    Console.Clear();
                    gold = player.Gold();
                    Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
                    Console.WriteLine($"[보유 골드]\n{gold} G\n");

                    Console.WriteLine($"[아이템 목록]");
                    if (stock_length != 0) { for (int k = 0; k < stock_length; k++) { Console.Write($"- {j} "); stock[k].item_Shop(); j++; } }
                    Console.WriteLine("0. 나가기\n");


                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    if (i == -1) Console.WriteLine("잘못된 입력입니다");
                    else if (i == -2) Console.WriteLine("골드가 부족합니다");
                    else if (i == -3) Console.WriteLine("이미 구입한 아이템 입니다");

                    i = int.Parse(Console.ReadLine());

                    if (i == 0) break;
                    else if (i <= stock.Length)
                    {
                        if (stock[i-1].getBought() == false)
                        {
                            if (stock[i - 1].getPrice() <= player.Gold()) { 
                                stock[i - 1].buy(); 
                                player.lose_Gold(stock[i - 1].getPrice()); 
                                player.get(stock[i - 1]);
                                SaveLoad.Save(player, "player.dat");
                                SaveLoad.SaveShop(shop, "shop.dat");
                            }
                            else { i = -2; }
                        }
                        else { i = -3; }
                    }
                    else i = -1;

                    j = 1;
                }
            }
        }




        public static class SaveLoad
        {
            public static void Save(Character player, string fileName)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, player);
                }
            }

            public static Character Load(string fileName)
            {
                if (!File.Exists(fileName)) return new Character();

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (Character)formatter.Deserialize(fs);
                }
            }

            public static void SaveShop(Shop shop, string fileName)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, shop);
                }
            }

            public static Shop LoadShop(string fileName)
            {
                if (!File.Exists(fileName)) return new Shop();

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (Shop)formatter.Deserialize(fs);
                }
            }
        }


        static int shop_length = 10;

        static Character player = SaveLoad.Load("player.dat");
        static Shop shop = SaveLoad.LoadShop("shop.dat");

        static void Main(string[] args)
        {
            int player_choice = 0;
 


            while (true)
            {
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전입장\n5. 휴식하기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                if (player_choice == -1) { Console.WriteLine("잘못된 입력입니다"); }
                player_choice = int.Parse(Console.ReadLine());

                if (player_choice == 1) { player.stat(); }
                else if (player_choice == 2) { player.inven(); }
                else if (player_choice == 3) { shop.shopping(); }
                else if (player_choice == 4) { player.dungeon(); }
                else if (player_choice == 5) { player.restScreen(); }
                else { player_choice = -1; }

                Console.Clear();
            }
        }
    }
}
