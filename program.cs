using System;
using System.Collections.Generic;
using System.Linq;

namespace GeographyQuizApp
{
    // Base class for all question types
    public abstract class Question
    {
        public string Text { get; set; } = string.Empty;

        public abstract bool CheckAnswer(string? userAnswer);
        public abstract string GetCorrectAnswer();
        public abstract void DisplayQuestion();
    }

    // Derived class for Multiple Choice Questions
    public class MultipleChoiceQuestion : Question
    {
        public List<string> Options { get; set; } = new List<string>();
        public string CorrectOption { get; set; } = string.Empty;

        public override bool CheckAnswer(string? userAnswer)
        {
            return !string.IsNullOrWhiteSpace(userAnswer) &&
                   string.Equals(userAnswer.Trim(), CorrectOption, StringComparison.OrdinalIgnoreCase);
        }

        public override string GetCorrectAnswer()
        {
            return CorrectOption;
        }

        public override void DisplayQuestion()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Text);
            Console.ResetColor();
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {Options[i]}");
            }
            Console.WriteLine();
        }
    }

    // Derived class for Open-ended Questions
    public class OpenEndedQuestion : Question
    {
        public List<string> AcceptedAnswers { get; set; } = new List<string>();

        public override bool CheckAnswer(string? userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
                return false;

            return AcceptedAnswers.Contains(userAnswer.Trim(), StringComparer.OrdinalIgnoreCase);
        }

        public override string GetCorrectAnswer()
        {
            return string.Join(", ", AcceptedAnswers);
        }

        public override void DisplayQuestion()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Text);
            Console.ResetColor();
        }
    }

    // Derived class for True/False Questions
    public class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer { get; set; }

        public override bool CheckAnswer(string? userAnswer)
        {
            return bool.TryParse(userAnswer?.Trim(), out bool result) && result == CorrectAnswer;
        }

        public override string GetCorrectAnswer()
        {
            return CorrectAnswer ? "True" : "False";
        }

        public override void DisplayQuestion()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Text);
            Console.ResetColor();
            Console.WriteLine("  True or False?");
            Console.WriteLine();
        }
    }

    class Program
    {
        private static List<Question> Questions = new List<Question>();
        private static List<(Question, bool)> IncorrectAnswers = new List<(Question, bool)>(); // Track failed questions
        private static List<bool> UserAnswers = new List<bool>(); // To store user's answers for corrections

        static void Main()
        {
            // Predefined questions
            InitializeQuestions();

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=== Welcome to the Geography Quiz App ===\n");
                Console.ResetColor();

                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Create a Question");
                Console.WriteLine("2. Manage Questions");
                Console.WriteLine("3. Play the Quiz");
                Console.WriteLine("4. View Corrections");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddQuestion();
                        break;
                    case "2":
                        ManageQuestions();
                        break;
                    case "3":
                        PlayQuiz();
                        break;
                    case "4":
                        ViewCorrections();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        // Initialize predefined questions
        private static void InitializeQuestions()
        {
            Questions.Add(new MultipleChoiceQuestion
            {
                Text = "What is the capital of Japan?",
                Options = new List<string> { "Beijing", "Seoul", "Tokyo", "Bangkok" },
                CorrectOption = "Tokyo"
            });

            Questions.Add(new OpenEndedQuestion
            {
                Text = "What is the longest river in the world?",
                AcceptedAnswers = new List<string> { "Nile", "Amazon" }
            });

            Questions.Add(new TrueFalseQuestion
            {
                Text = "The Great Wall of China is visible from space.",
                CorrectAnswer = false
            });

            Questions.Add(new MultipleChoiceQuestion
            {
                Text = "Which country is known as the Land of the Rising Sun?",
                Options = new List<string> { "China", "Japan", "Thailand", "India" },
                CorrectOption = "Japan"
            });

            Questions.Add(new OpenEndedQuestion
            {
                Text = "Which continent is known as the Dark Continent?",
                AcceptedAnswers = new List<string> { "Africa" }
            });

            Questions.Add(new TrueFalseQuestion
            {
                Text = "Australia is both a country and a continent.",
                CorrectAnswer = true
            });

            Questions.Add(new MultipleChoiceQuestion
            {
                Text = "Which is the smallest country in the world?",
                Options = new List<string> { "Monaco", "Nauru", "Vatican City", "San Marino" },
                CorrectOption = "Vatican City"
            });

            Questions.Add(new OpenEndedQuestion
            {
                Text = "What is the capital city of Canada?",
                AcceptedAnswers = new List<string> { "Ottawa" }
            });

            Questions.Add(new TrueFalseQuestion
            {
                Text = "The Amazon Rainforest is located in Africa.",
                CorrectAnswer = false
            });

            Questions.Add(new MultipleChoiceQuestion
            {
                Text = "Which ocean is the largest?",
                Options = new List<string> { "Atlantic Ocean", "Indian Ocean", "Arctic Ocean", "Pacific Ocean" },
                CorrectOption = "Pacific Ocean"
            });
        }

        // Add a new question
        private static void AddQuestion()
        {
            Console.Clear();
            Console.WriteLine("=== Add a New Question ===");

            Console.WriteLine("Select the type of question:");
            Console.WriteLine("1. Multiple Choice");
            Console.WriteLine("2. Open-ended");
            Console.WriteLine("3. True/False");
            Console.Write("Enter your choice: ");
            string? questionType = Console.ReadLine();

            Question newQuestion = questionType switch
            {
                "1" => CreateMultipleChoiceQuestion(),
                "2" => CreateOpenEndedQuestion(),
                "3" => CreateTrueFalseQuestion(),
                _ => null
            };

            if (newQuestion != null)
            {
                Questions.Add(newQuestion);
                Console.WriteLine("Question added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid choice. Question not added.");
            }

            Console.ReadLine();
        }

        // Create a multiple choice question
        private static MultipleChoiceQuestion CreateMultipleChoiceQuestion()
        {
            var question = new MultipleChoiceQuestion();
            Console.Write("Enter the question text: ");
            question.Text = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter 4 options:");
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Option {i + 1}: ");
                question.Options.Add(Console.ReadLine() ?? string.Empty);
            }

            Console.Write("Enter the correct option (1-4): ");
            int correctOptionIndex = int.Parse(Console.ReadLine() ?? "0") - 1;
            question.CorrectOption = question.Options[correctOptionIndex];

            return question;
        }

        // Create an open-ended question
        private static OpenEndedQuestion CreateOpenEndedQuestion()
        {
            var question = new OpenEndedQuestion();
            Console.Write("Enter the question text: ");
            question.Text = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter acceptable answers (separated by commas): ");
            question.AcceptedAnswers = Console.ReadLine()?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>();

            return question;
        }

        // Create a true/false question
        private static TrueFalseQuestion CreateTrueFalseQuestion()
        {
            var question = new TrueFalseQuestion();
            Console.Write("Enter the question text: ");
            question.Text = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter the correct answer (True/False): ");
            bool correctAnswer = bool.Parse(Console.ReadLine() ?? "false");
            question.CorrectAnswer = correctAnswer;

            return question;
        }

        // Manage existing questions (Edit/Delete)
        private static void ManageQuestions()
        {
            Console.Clear();
            Console.WriteLine("=== Manage Questions ===");
            Console.WriteLine("1. Edit a Question");
            Console.WriteLine("2. Delete a Question");
            Console.WriteLine("3. Return to Main Menu");
            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EditQuestion();
                    break;
                case "2":
                    DeleteQuestion();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }

        // Edit a question
        private static void EditQuestion()
        {
            DisplayQuestions();
            Console.Write("Enter the question number to edit: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= Questions.Count)
            {
                var question = Questions[index - 1];
                Console.WriteLine($"Editing: {question.Text}");
                Console.Write("Enter new question text: ");
                question.Text = Console.ReadLine() ?? string.Empty;

                // Recreate the question if necessary based on its type
                if (question is MultipleChoiceQuestion mcq)
                {
                    Console.WriteLine("Enter 4 options:");
                    for (int i = 0; i < mcq.Options.Count; i++)
                    {
                        Console.Write($"Option {i + 1}: ");
                        mcq.Options[i] = Console.ReadLine() ?? string.Empty;
                    }
                    Console.Write("Enter the correct option (1-4): ");
                    int correctOptionIndex = int.Parse(Console.ReadLine() ?? "0") - 1;
                    mcq.CorrectOption = mcq.Options[correctOptionIndex];
                }
                else if (question is OpenEndedQuestion openEnded)
                {
                    Console.WriteLine("Enter acceptable answers (separated by commas): ");
                    openEnded.AcceptedAnswers = Console.ReadLine()?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>();
                }
                else if (question is TrueFalseQuestion trueFalse)
                {
                    Console.Write("Enter the correct answer (True/False): ");
                    bool correctAnswer = bool.Parse(Console.ReadLine() ?? "false");
                    trueFalse.CorrectAnswer = correctAnswer;
                }

                Console.WriteLine("Question updated successfully!");
            }
            else
            {
                Console.WriteLine("Invalid question number.");
            }

            Console.ReadLine();
        }

        // Delete a question
        private static void DeleteQuestion()
        {
            DisplayQuestions();
            Console.Write("Enter the question number to delete: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= Questions.Count)
            {
                Questions.RemoveAt(index - 1);
                Console.WriteLine("Question deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid question number.");
            }

            Console.ReadLine();
        }

        // Display all available questions
        private static void DisplayQuestions()
        {
            Console.Clear();
            Console.WriteLine("=== Available Questions ===");
            for (int i = 0; i < Questions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Questions[i].Text}");
            }
            Console.WriteLine();
        }

        // Play the quiz game with each question on a different page
        private static void PlayQuiz()
        {
            Console.Clear();
            Console.WriteLine("=== Let's Play the Geography Quiz! ===\n");

            int correctAnswersCount = 0;
            UserAnswers.Clear(); // Reset user answers
            IncorrectAnswers.Clear(); // Clear incorrect answers

            // Iterate over all questions and ask one by one, pausing after each question
            foreach (var question in Questions)
            {
                question.DisplayQuestion();
                string? userAnswer = null;

                // Ensure user answers all questions before proceeding
                while (string.IsNullOrWhiteSpace(userAnswer))
                {
                    Console.Write("Enter your answer: ");
                    userAnswer = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(userAnswer))
                    {
                        Console.WriteLine("You must provide an answer.");
                    }
                }

                bool isCorrect = question.CheckAnswer(userAnswer);
                UserAnswers.Add(isCorrect);

                if (isCorrect)
                {
                    correctAnswersCount++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Correct!\n");
                }
                else
                {
                    IncorrectAnswers.Add((question, isCorrect)); // Track failed questions
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Incorrect. The correct answer was: {question.GetCorrectAnswer()}\n");
                }
                Console.ResetColor();

                // Pause to show the next question
                Console.WriteLine("Press any key to go to the next question...");
                Console.ReadKey();
                Console.Clear();
            }

            // Final score and congratulatory message
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (correctAnswersCount == Questions.Count)
            {
                Console.WriteLine("Congratulations! You answered all the questions correctly!");
            }
            else
            {
                Console.WriteLine($"You answered {correctAnswersCount} out of {Questions.Count} questions correctly.");
            }
            Console.ResetColor();
            Console.ReadLine();
        }

        // View corrections for failed questions
        private static void ViewCorrections()
        {
            if (IncorrectAnswers.Count == 0)
            {
                Console.WriteLine("You answered all questions correctly! Great job!");
            }
            else
            {
                Console.WriteLine("=== Corrections ===");
                foreach (var (question, _) in IncorrectAnswers)
                {
                    Console.WriteLine($"Question: {question.Text}");
                    Console.WriteLine($"Correct Answer: {question.GetCorrectAnswer()}\n");
                }
            }

            Console.ReadLine();
        }
    }
}
