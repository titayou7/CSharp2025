// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;

namespace StudentManagementSystem
{
    // 成绩等级枚举
    public enum Grade
    {
        // TODO: 定义成绩等级 F(0), D(60), C(70), B(80), A(90)
        F = 0,
        D = 60,
        C = 70,
        B = 80,
        A = 90
    }

    // 泛型仓储接口
    public interface IRepository<T>
    {
        // TODO: 定义接口方法
        // Add(T item)
        // Remove(T item) 返回bool
        // GetAll() 返回List<T>
        // Find(Func<T, bool> predicate) 返回List<T>
        void Add(T item);
        bool Remove(T item);
        List<T> GetAll();
        List<T> Find(Func<T, bool> predicate);
    }

    // 学生类
    public class Student : IComparable<Student>
    {
        // TODO: 定义字段 StudentId, Name, Age
        public string StudentId;
        public string Name;
        public int Age;

        public Student(string? studentId, string? name, int? age)
        {
            // TODO: 实现构造方法，包含参数验证（空值检查）
            this.StudentId = studentId ?? "0000000042";
            this.Name = name ?? "牢大";
            this.Age = age ?? 33;
        }

        public override string ToString()
        {
            // TODO: 返回格式化的学生信息字符串
            return $"学号：\t{this.StudentId}\n姓名：\t{this.Name}\n年龄：\t{this.Age}";
            
        }

        // TODO: 实现IComparable接口，按学号排序
        // 提示：使用string.Compare方法
        public int CompareTo(Student? other)
        {
            return this.StudentId.CompareTo(other.StudentId);
        }

        public override bool Equals(object? obj)
        {
            return obj is Student student && StudentId == student.StudentId;
        }

        public override int GetHashCode()
        {
            return StudentId?.GetHashCode() ?? 0;
        }
    }

    // 成绩类
    public class Score
    {
        // TODO: 定义字段 Subject, Points
        public string Subject;
        public double Points;

        public Score(string? subject, double? points)
        {
            // TODO: 实现构造方法，包含参数验证
            this.Subject = subject ?? "wjf(W)" ;
            this.Points = points ?? 0.0 ;
        }

        public override string ToString()
        {
            // TODO: 返回格式化的成绩信息
            return $"科目：\t{this.Subject}\n成绩：\t{this.Points}";
        }
    }

    // 学生管理类
    public class StudentManager : IRepository<Student>
    {
        // TODO: 定义私有字段存储学生列表
        // 提示：使用List<Student>存储
        private List<Student> StudentList;
        public StudentManager()
        {
            StudentList = new List<Student>();
        }
        public void Add(Student item)
        {
            // TODO: 实现添加学生的逻辑
            // 1. 参数验证
            // 2. 添加到列表
            if (item is not null)
            {
                this.StudentList.Add(item);
            }

            
        }

        public bool Remove(Student item)
        {
            // TODO: 实现Remove方法
            return this.StudentList.Remove(item);
        }

        public List<Student> GetAll()
        {
            // TODO: 返回学生列表的副本
            return StudentList;
        }

        public List<Student> Find(Func<Student, bool> predicate)
        {
            // TODO: 使用foreach循环查找符合条件的学生
            List<Student> l = new List<Student>();
            foreach (var item in this.StudentList)
            {
                if (predicate(item))
                {
                    l.Add(item);
                }
            }
            return l;
        }

        // 查找年龄在指定范围内的学生
        public List<Student> GetStudentsByAge(int minAge, int maxAge)
        {
            // TODO: 使用foreach循环和if判断实现年龄范围查询
            List<Student> r = new List<Student>();
            foreach (var item in this.StudentList)
            {
                if (minAge <= item.Age && item.Age <= maxAge)
                {
                    r.Add(item);
                }
            }
            return r;
        }
    }

    // 成绩管理类
    public class ScoreManager
    {
        // TODO: 定义私有字段存储成绩字典
        // 提示：使用Dictionary<string, List<Score>>存储
        private Dictionary<string, List<Score>> ScoreDict;
        public ScoreManager()
        {
            ScoreDict = new Dictionary<string, List<Score>>();
        }

        public void AddScore(string studentId, Score score)
        {
            // TODO: 实现添加成绩的逻辑
            // 1. 参数验证
            // 2. 初始化学生成绩列表（如不存在）
            // 3. 添加成绩
            if (studentId is null || score is null)
            {
                return;
            }
            if (!this.ScoreDict.ContainsKey(studentId))
            {
                this.ScoreDict[studentId] = new List<Score>();
            }

            this.ScoreDict[studentId].Add(score);
            

        }

        public List<Score> GetStudentScores(string studentId)
        {
            // TODO: 获取指定学生的所有成绩
            return ScoreDict[studentId];
        }

        public double CalculateAverage(string studentId)
        {
            // TODO: 计算指定学生的平均分
            // 提示：使用foreach循环计算总分，然后除以科目数

            double total = 0;
            double count = 0;
            foreach (var item in ScoreDict[studentId])
            {
                total += item.Points;
                count++;
            }
            return total / count;
        }

        // TODO: 使用模式匹配实现成绩等级转换
        static public Grade GetGrade(double score)
        {
            switch (score)
            {
                case >= 0.0 and < 60.0:
                    return Grade.F;
                case >= 60.0 and < 70.0:
                    return Grade.D;
                case >= 70.0 and < 80.0:
                    return Grade.C;
                case >= 80.0 and < 90.0:
                    return Grade.B;
                case >= 90.0 and < 100.0:
                    return Grade.A;
                default:
                    return Grade.F;
            }
        }

        public List<(string StudentId, double Average)> GetTopStudents(int count)
        {
            // TODO: 使用简单循环获取平均分最高的学生
            // 提示：可以先计算所有学生的平均分，然后排序取前count个
            List<(string StudentId, double Average)> TopStudents = new List<(string StudentId, double Average)>();
            foreach (var item in this.ScoreDict)
            {
                double average = this.CalculateAverage(item.Key);
                TopStudents.Add((item.Key, average));

            }
            TopStudents.Sort((a, b) => { return b.Item2.CompareTo(a.Item2); });
            if (TopStudents.Count > count)
            {
                TopStudents.RemoveRange(count, TopStudents.Count - count);
            }
            return TopStudents;
        }

        public Dictionary<string, List<Score>> GetAllScores()
        {
            return ScoreDict;
        }
    }

    // 数据管理类
    public class DataManager
    {
        public void SaveStudentsToFile(List<Student> students, string filePath)
        {
            // TODO: 实现保存学生数据到文件
            // 提示：使用StreamWriter，格式为CSV
            try
            {
                // 在这里实现文件写入逻辑
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var item in students)
                    {
                        writer.WriteLine($"{item.StudentId},{item.Name},{item.Age}");
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存文件时发生错误: {ex.Message}");
            }
        }

        public List<Student> LoadStudentsFromFile(string filePath)
        {
            List<Student> students = new List<Student>();

            // TODO: 实现从文件读取学生数据
            // 提示：使用StreamReader，解析CSV格式
            try
            {
                // 在这里实现文件读取逻辑
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line = new string("");
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');

                        string? part1 = parts[0]; // "2021001"
                        string? part2 = parts[1]; // "张三"
                        int? part3 = int.Parse(parts[2]); // 95.5

                        Student stud = new Student(part1, part2, part3);
                        students.Add(stud);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取文件时发生错误: {ex.Message}");
            }
            
            return students;
        }
    }

    // 主程序
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 学生成绩管理系统 ===\n");

            // 创建管理器实例
            var studentManager = new StudentManager();
            var scoreManager = new ScoreManager();
            var dataManager = new DataManager();

            try
            {
                // 1. 学生数据（共3个学生）
                Console.WriteLine("1. 添加学生信息:");
                studentManager.Add(new Student("2021001", "张三", 20));
                studentManager.Add(new Student("2021002", "李四", 19));
                studentManager.Add(new Student("2021003", "王五", 21));
                System.Threading.Thread.Sleep(2000); // 阻塞 2000 毫秒（2 秒）
                Console.WriteLine("学生信息添加完成");
                System.Threading.Thread.Sleep(500); // 阻塞 500 毫秒（0.5 秒）
                // 2. 成绩数据（每个学生各2门课程）
                Console.WriteLine("\n2. 添加成绩信息:");
                scoreManager.AddScore("2021001", new Score("数学", 95.5));
                scoreManager.AddScore("2021001", new Score("英语", 87.0));
                
                scoreManager.AddScore("2021002", new Score("数学", 78.5));
                scoreManager.AddScore("2021002", new Score("英语", 85.5));
                
                scoreManager.AddScore("2021003", new Score("数学", 88.0));
                scoreManager.AddScore("2021003", new Score("英语", 92.0));
                System.Threading.Thread.Sleep(2000); // 阻塞 2000 毫秒（2 秒）
                Console.WriteLine("成绩信息添加完成");
                System.Threading.Thread.Sleep(500); // 阻塞 500 毫秒（0.5 秒）

                // 3. 测试年龄范围查询
                Console.WriteLine("\n3. 查找年龄在19-20岁的学生:");
                System.Threading.Thread.Sleep(200); // 阻塞 200 毫秒（0.2 秒）
                // TODO: 调用GetStudentsByAge方法并显示结果
                List<Student> StudentsAgeIn19_20 = studentManager.GetStudentsByAge(19, 20);
                foreach (var item in StudentsAgeIn19_20)
                {
                    Console.WriteLine(item);
                    System.Threading.Thread.Sleep(200); // 阻塞 200 毫秒（0.2 秒）
                }
                System.Threading.Thread.Sleep(500); // 阻塞 500 毫秒（0.5 秒）

                // 4. 显示学生成绩统计
                Console.WriteLine("\n4. 学生成绩统计:");
                System.Threading.Thread.Sleep(200); // 阻塞 200 毫秒（0.2 秒）
                // TODO: 遍历所有学生，显示其成绩、平均分和等级
                foreach (var item in studentManager.GetAll())
                {
                    Console.WriteLine($"姓名:{item.Name}");
                    System.Threading.Thread.Sleep(200); // 阻塞 200 毫秒（0.2 秒）
                    foreach (var item2 in scoreManager.GetAllScores()[item.StudentId])
                    {
                        Console.WriteLine($"学科:\t{item2.Subject}\t\t分数:{item2.Points}");
                        System.Threading.Thread.Sleep(200); // 阻塞 200 毫秒（0.2 秒）
                    }
                    Console.WriteLine($"平均分:\t{scoreManager.CalculateAverage(item.StudentId)}");
                    System.Threading.Thread.Sleep(200); // 阻塞 200 毫秒（0.2 秒）
                    Console.WriteLine($"等级:\t{ScoreManager.GetGrade(scoreManager.CalculateAverage(item.StudentId))}");
                    System.Threading.Thread.Sleep(200); // 阻塞 200 毫秒（0.2 秒）
                }

                // 5. 显示排名（简化版）
                Console.WriteLine("\n5. 平均分最高的学生:");
                // TODO: 调用GetTopStudents(1)方法显示第一名
                Console.WriteLine($"平均分最高的学生:\t{scoreManager.GetTopStudents(1)[0].Item1}\n分数:\t\t\t{scoreManager.GetTopStudents(1)[0].Item2}");

                // 6. 文件操作
                Console.WriteLine("\n6. 数据持久化演示:");
                // TODO: 保存和读取学生文件
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序执行过程中发生错误: {ex.Message}");
            }

            Console.WriteLine("\n程序执行完毕，按任意键退出...");
            Console.ReadKey();
        }
    }
}