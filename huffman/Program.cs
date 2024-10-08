using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huffman
{
    class HuffmanNode
    {
        public char Character { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }

        public HuffmanNode(char character, int frequency) //ağacın düğümlerini temsil eden bir metod oluşturuyoruz 
        {
            Character = character;
            Frequency = frequency;
        }

        // Leaf node kontrolü
        public bool IsLeaf()
        {
            return Left == null && Right == null;  //düğüm yaprak mı kontrolü yapılıyor
        }
    }
    class HuffmanCoding
    {
        public Dictionary<char, string> Encode(string text) //kodları oluşturan metod 
        {
            var frequency = CalculateFrequency(text); //frekans hesaplar
            var root = BuildHuffmanTree(frequency); //huffman ağacını oluşturur
            var huffmanCode = new Dictionary<char, string>();// her karakterin huffman kodunu tutan sözlük
            GenerateHuffmanCodes(root, "", huffmanCode); // ağacı dolaşarak her karakter için  kodlar oluşturur.

            return huffmanCode;
        }
        public Dictionary<char, int> CalculateFrequency(string text)   // frekans hesaplama
        {
            var frequency = new Dictionary<char, int>();   // boş bir frekans sözlük oluşturuyoruz.
            foreach (var ch in text)  // döngü ile verilen metindeki her bir karakteri döndürüyor.
            {
         

                if (!frequency.ContainsKey(ch)) // karakter sözlükte yok ise sıfır olarak eklenir
                    frequency[ch] = 0;
                frequency[ch]++; //varsa değeri bir arttırılır
            }

            return frequency; //hesaplanan frekansları döndürür.
        }

        private HuffmanNode BuildHuffmanTree(Dictionary<char, int> frequency)
        {
            var priorityQueue = new List<Tuple<int, HuffmanNode>>(); // frekanslara göre düğümleri öncelik sırasına göre sıralayacacağımız bir liste

            // Her karakter ve frekans için düğümler oluşturuluyor
            foreach (var kvp in frequency)
            { //frekeans sözlüğündeki her karakter için bir düğüm oluşturuluyor ve  kuyruğa ekleniyor.
                priorityQueue.Add(Tuple.Create(kvp.Value, new HuffmanNode(kvp.Key, kvp.Value)));
                
            }

            // Frekansa göre küçükten büyüğe sıralama yapılıyor
            priorityQueue = priorityQueue.OrderBy(x => x.Item1).ToList();

            // Ağacı oluşturmaya başlıyoruz
            while (priorityQueue.Count > 1) // kuyrukta bir düğüm kalana kadar devam eder
            {
                var left = priorityQueue[0].Item2; // En küçük frekanslı düğümü alıyoruz sola ekliyoruz
                priorityQueue.RemoveAt(0); // Aldığımız düğümü listeden çıkarıyoruz

                var right = priorityQueue[0].Item2; // İkinci en küçük frekanslı düğümü alıyoruz
                priorityQueue.RemoveAt(0); // Onu da listeden çıkarıyoruz

                // İki düğümün frekanslarını topluyoruz
                var sum = left.Frequency + right.Frequency;

                // Yeni bir düğüm oluşturuyoruz, bu düğüm iki küçük düğümün toplam frekansı olacak
                var newNode = new HuffmanNode('*', sum)
                {
                    Left = left, // Soluna küçük frekanslı düğümü yerleştiriyoruz
                    Right = right // Sağına ikinci en  küçük frekanslı düğümü yerleştiriyoruz
                };

                // Yeni düğümü öncelik sırasına göre listeye ekliyoruz
                priorityQueue.Add(Tuple.Create(sum, newNode));
                priorityQueue = priorityQueue.OrderBy(x => x.Item1).ToList(); //kuyruğu frekansa göre tekrar sıralıyoruz
            }

            // Tek kalan düğüm kök düğüm olacak
            return priorityQueue[0].Item2;
        }
        private void GenerateHuffmanCodes(HuffmanNode node, string code, Dictionary<char, string> huffmanCode)//ağacı dolaşır 0-1 ataması
        {                                  //mevcut düğüm
            if (node == null) // düğümün boş olup olmadığını kontrol eder.
                return;

   
            if (node.IsLeaf()) //eğer düğüm yaprak düğümse karakterin huffman kodunu sözlüğe ekler
            {
                huffmanCode[node.Character] = code;
            }

            GenerateHuffmanCodes(node.Left, code + "0", huffmanCode); //sol çocuğa geçtiğinde 0 kodunu ekler
            GenerateHuffmanCodes(node.Right, code + "1", huffmanCode); // sağ alt çocuğa geçtiğinde 1 kodunu ekler
        }

        public string Compress(string text, Dictionary<char, string> huffmanCode)
        {
            string compressedText = ""; //yenı kod
            foreach (var ch in text)
            {
      
                compressedText += huffmanCode[ch]; //  huffman kod sözlüğündeki karakterin huffman kodu compresstext stiringine eklenir. her karakter için tekrarlanır
            }

            return compressedText;
        }

        public void PrintFrequencies(Dictionary<char, int> frequency)
        {
            Console.WriteLine("Karakter Frekansları:");
            foreach (var kvp in frequency) //karakterlerin frekanslarını ekrana yazdırır.
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            string text = "aaaabbbbcccc"; // metni gir

            HuffmanCoding huffmanCoding = new HuffmanCoding(); //metin sıkıştırma ve kodları üreten sınıfı çağır
            var frequency = huffmanCoding.CalculateFrequency(text); //frekansları hesaplayan metodu çağır
            huffmanCoding.PrintFrequencies(frequency); // Frekansları yazdır

            var huffmanCode = huffmanCoding.Encode(text); //huffman kodlarını üretne metodu çeğır

            Console.WriteLine("\nHuffman Kodları:");
            foreach (var kvp in huffmanCode) //huffman kodlarını ekrana yazdırı
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }

            string compressedText = huffmanCoding.Compress(text, huffmanCode); // verilen texte her karakterin huffman kodu ile yerini değiştirir
            Console.WriteLine($"\nSıkıştırılmış Metin : {compressedText}"); //sıkıştırılmış metni yazdırır.
        }

    }
}
