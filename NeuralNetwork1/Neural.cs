using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using AForge;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace NeuralNetwork1
{

    public enum FigureType { zero=0, one, two, three, four, five, six, seven, eight, nine, Undef };
    /// <summary>
    /// Класс для хранения образа – входной массив сигналов на сенсорах, выходные сигналы сети, и прочее
    /// </summary>
    public class Sample
    {
        /// <summary>
        /// Входной вектор
        /// </summary>
        public double[] input = null;

        /// <summary>
        /// Выходной вектор, задаётся извне как результат распознавания
        /// </summary>
        public double[] output = null;

        /// <summary>
        /// Вектор ошибки, вычисляется по какой-нибудь хитрой формуле
        /// </summary>
        public double[] error = null;

        /// <summary>
        /// Действительный класс образа. Указывается учителем
        /// </summary>
        public FigureType actualClass;

        /// <summary>
        /// Распознанный класс - определяется после обработки
        /// </summary>
        public FigureType recognizedClass;

        /// <summary>
        /// Конструктор образа - на основе входных данных для сенсоров, при этом можно указать класс образа, или не указывать
        /// </summary>
        /// <param name="inputValues"></param>
        /// <param name="sampleClass"></param>
        public Sample(double[] inputValues, int classesCount, FigureType sampleClass = FigureType.Undef)
        {
            //  Клонируем массивчик
            input = (double[]) inputValues.Clone();
            output = new double[classesCount];
            if (sampleClass != FigureType.Undef) output[(int)sampleClass] = 1;


            recognizedClass = FigureType.Undef;
            actualClass = sampleClass;
        }

        /// <summary>
        /// Обработка реакции сети на данный образ на основе вектора выходов сети
        /// </summary>
        public void processOutput()
        {
            if (error == null)
                error = new double[output.Length];
            
            //  Нам так-то выход не нужен, нужна ошибка и определённый класс
            recognizedClass = 0;
            for(int i = 0; i < output.Length; ++i)
            {
                error[i] = ((i == (int) actualClass ? 1 : 0) - output[i]);
                if (output[i] > output[(int)recognizedClass]) recognizedClass = (FigureType)i;
            }
        }

        /// <summary>
        /// Вычисленная суммарная квадратичная ошибка сети. Предполагается, что целевые выходы - 1 для верного, и 0 для остальных
        /// </summary>
        /// <returns></returns>
        public double EstimatedError()
        {
            double Result = 0;
            for (int i = 0; i < output.Length; ++i)
                Result += Math.Pow(error[i], 2);
            return Result;
        }

        /// <summary>
        /// Добавляет к аргументу ошибку, соответствующую данному образу (не квадратичную!!!)
        /// </summary>
        /// <param name="errorVector"></param>
        /// <returns></returns>
        public void updateErrorVector(double[] errorVector)
        {
            for (int i = 0; i < errorVector.Length; ++i)
                errorVector[i] += error[i];
        }

        /// <summary>
        /// Представление в виде строки
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "Sample decoding : " + actualClass.ToString() + "(" + ((int)actualClass).ToString() + "); " + Environment.NewLine + "Input : ";
            for (int i = 0; i < input.Length; ++i) result += input[i].ToString() + "; ";
            result += Environment.NewLine + "Output : ";
            if (output == null) result += "null;";
            else
                for (int i = 0; i < output.Length; ++i) result += output[i].ToString() + "; ";
            result += Environment.NewLine + "Error : ";

            if (error == null) result += "null;";
            else
                for (int i = 0; i < error.Length; ++i) result += error[i].ToString() + "; ";
            result += Environment.NewLine + "Recognized : " + recognizedClass.ToString() + "(" + ((int)recognizedClass).ToString() + "); " + Environment.NewLine;


            return result;
        }
        
        /// <summary>
        /// Правильно ли распознан образ
        /// </summary>
        /// <returns></returns>
        public bool Correct() { return actualClass == recognizedClass; }
    }
    
    /// <summary>
    /// Выборка образов. Могут быть как классифицированные (обучающая, тестовая выборки), так и не классифицированные (обработка)
    /// </summary>
    public class SamplesSet : IEnumerable
    {
        /// <summary>
        /// Накопленные обучающие образы
        /// </summary>
        public List<Sample> samples = new List<Sample>();
        
        /// <summary>
        /// Добавление образа к коллекции
        /// </summary>
        /// <param name="image"></param>
        public void AddSample(Sample image)
        {
            samples.Add(image);
        }
        public int Count { get { return samples.Count; } }

        public IEnumerator GetEnumerator()
        {
            return samples.GetEnumerator();
        }

        /// <summary>
        /// Реализация доступа по индексу
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Sample this[int i]
        {
            get { return samples[i]; }
            set { samples[i] = value; }
        }

        public double ErrorsCount()
        {
            double correct = 0;
            double wrong = 0;
            foreach (var sample in samples)
                if (sample.Correct()) ++correct; else ++wrong;
            return correct / (correct + wrong);
        }
        // Тут бы ещё сохранение в файл и чтение сделать, вообще классно было бы
    }

    public class MyNetwork
    {
        public MyNetwork(Func<double, double> func, int inputsCount, int[] neuronsCount, double learningRate = 0.25)
        {
            this.func = func;
            this.learningRate = learningRate;
            inputs = new double[inputsCount+1];
            outputs = new double[neuronsCount[neuronsCount.Length - 1]];
            layersValue = new double[neuronsCount.Length-1][];

            weights = new double[neuronsCount.Length][][];
            int prevAmount = inputsCount;
            int nextAmount = neuronsCount[0];

            for (int i = 0; i < neuronsCount.Length-1; i++)
            {
                layersValue[i] = new double[neuronsCount[i] + 1];
                layersValue[i][neuronsCount[i]] = 1.1;
                //weights[i] = new double[prevAmount+1][];
                //for (int j = 0; j < prevAmount; ++j)
                //    weights[i][j] = new double[nextAmount];
                weights[i] = new double[nextAmount][];
                for (int j = 0; j < nextAmount; ++j)
                    weights[i][j] = new double[prevAmount+1];
                    prevAmount = nextAmount;
                    nextAmount = neuronsCount[i + 1];
            }

            //prevAmount = neuronsCount[neuronsCount.Length - 2];
            //nextAmount = outputs.Length;

            var lastWeight = neuronsCount.Length - 1;

            weights[lastWeight] = new double[nextAmount][];
            for (int j = 0; j < nextAmount; ++j)
                weights[lastWeight][j] = new double[prevAmount+1];
        }

        Func<double, double> func; //функция активации
        
        public double[] inputs; //начальные значения
        public double[] outputs; // конечные значения

        public double[][] layersValue; //значения на нейронах при вычислении || номер слоя, номер нейрона
        public double[][][] weights;   //матрицы весов между нейронами при вычислении || номер слоя, номер 2-го нейрона, номер 1-го нейрона

        public double learningRate;

        public void RandomizeWeights()
        {
            Random r = new Random();
            System.Threading.Tasks.Parallel.For(0, weights.Length, (i) =>
            {
                for (int j = 0; j < weights[i].Length; j++)
                    for (int k = 0; k < weights[i][j].Length; k++)
                        weights[i][j][k] = r.NextDouble() * Math.Sign(r.Next())*0.01;
            });
            //for (int i = 0; i < weights.Length; i++)
            //    for (int j = 0; j < weights[i].Length; j++)
            //        for (int k = 0; k < weights[i][j].Length; k++)
            //            weights[i][j][k] = r.NextDouble() * Math.Sign(r.Next())*0.01;
        }

        public double[] Compute(double[] input)
        {
            for (int i = 0; i < input.Length; i++)
                inputs[i] = input[i];

            inputs[inputs.Length - 1] = 1.1;

            //считаю value для 1 A слоя
            System.Threading.Tasks.Parallel.For(0, layersValue[0].Length - 1, (j) => 
            {
                layersValue[0][j] = 0;
                for (int k = 0; k < weights[0][j].Length; k++)
                    layersValue[0][j] += weights[0][j][k] * inputs[k];
                layersValue[0][j] = func(layersValue[0][j]);
            }
            );
            //for (int j = 0; j < layersValue[0].Length-1; j++)
            //{
            //    layersValue[0][j] = 0;
            //    for (int k = 0; k < weights[0][j].Length; k++)
            //        layersValue[0][j] += weights[0][j][k]*inputs[k];
            //    layersValue[0][j] = func(layersValue[0][j]);
            //}

            for (int i = 1; i < layersValue.Length; i++)
            {
                int i1 = i - 1;
                System.Threading.Tasks.Parallel.For(0, layersValue[i].Length - 1, (j) =>
                {
                    layersValue[i][j] = 0;
                    for (int k = 0; k < weights[i][j].Length; k++)
                        layersValue[i][j] += weights[i][j][k] * layersValue[i1][k];
                    layersValue[i][j] = func(layersValue[i][j]);
                });
                //for (int j = 0; j < layersValue[i].Length-1; j++)
                //{
                //    layersValue[i][j] = 0;
                //    for (int k = 0; k < weights[i][j].Length; k++)
                //        layersValue[i][j] += weights[i][j][k] * layersValue[i1][k];
                //    layersValue[i][j] = func(layersValue[i][j]);
                //}
            }

            int lvl = layersValue.Length - 1;
            int wl1 = weights.Length - 1;

            System.Threading.Tasks.Parallel.For(0, outputs.Length, (j) =>
            {
                outputs[j] = 0;
                for (int k = 0; k < weights[wl1][j].Length; k++)
                    outputs[j] += weights[wl1][j][k] * layersValue[lvl][k];
                outputs[j] = func(outputs[j]);
            });

            //for (int j = 0; j < outputs.Length; j++)
            //{
            //    outputs[j] = 0;
            //    for (int k = 0; k < weights[wl1][j].Length; k++)
            //        outputs[j] += weights[wl1][j][k] * layersValue[lvl][k];
            //    outputs[j] = func(outputs[j]);
            //}

            return outputs;
        }
    }

    public class MyNeuralNetwork : BaseNetwork
    {
        public double SigmoidFunc(double x)
        {
            return 1.0 / (1.0 + Math.Pow(Math.E, -x));
        }

        /// <summary>
        /// Реализация нейронной сети из Accord.NET
        /// </summary>
        private MyNetwork network = null;

        /// <summary>
        /// Значение ошибки при обучении единичному образцу. При обучении на наборе образов не используется
        /// </summary>
        public double desiredErrorValue = 0.0005;

        //  Секундомер спортивный, завода «Агат», измеряет время пробегания стометровки, ну и время затраченное на обучение тоже умеет
        public System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Конструктор сети с указанием структуры (количество слоёв и нейронов в них)
        /// </summary>
        /// <param name="structure"></param>
        public MyNeuralNetwork(int[] structure)
        {
            ReInit(structure);
        }

        public override void ReInit(int[] structure, double initialLearningRate = 0.25)
        {
            network = new MyNetwork(SigmoidFunc, structure[0],
                structure.Skip(1).ToArray<int>());

            network.RandomizeWeights();
        }

        public override int Train(Sample sample, bool parallel = true)
        {
            int iters = 0;

            sample.output = network.Compute(sample.input);
            sample.processOutput();

            while (sample.EstimatedError() > desiredErrorValue && iters < 100)
            {
                double[] deltas = new double[network.outputs.Length];

                //идем в обратную сторону
                if (parallel)
                {
                    System.Threading.Tasks.Parallel.For(0, network.outputs.Length, (i) =>
                    {
                        double der = (1 - network.outputs[i]) * network.outputs[i];
                        deltas[i] = ((i == (int)sample.actualClass ? 1 : 0) - network.outputs[i]) * der;
                    });
                }
                else
                {
                    for (int i = 0; i < network.outputs.Length; i++)
                    {
                        double der = (1 - network.outputs[i]) * network.outputs[i];
                        deltas[i] = ((i == (int)sample.actualClass ? 1 : 0) - network.outputs[i]) * der;
                    }
                }

                //A - слои

                for (int i = network.weights.Length - 1; i >= 1; i--)
                {
                    double[] nextdeltas = new double[network.weights[i][0].Length];
                    if (parallel)
                    {
                        System.Threading.Tasks.Parallel.For(0, network.weights[i][0].Length, (j) =>
                        {
                            double der = (1 - network.layersValue[i - 1][j]) * network.layersValue[i - 1][j];
                            double sum = 0;
                            for (int k = 0; k < deltas.Length; k++)
                                sum += deltas[k] * network.weights[i][k][j];
                            nextdeltas[j] = der * sum;
                            for (int k = 0; k < deltas.Length; k++)
                                network.weights[i][k][j] += network.learningRate * deltas[k] * network.layersValue[i - 1][j];
                        });
                    }
                    else
                    {
                        for (int j = 0; j < network.weights[i][0].Length; j++)
                        {
                            double der = (1 - network.layersValue[i - 1][j]) * network.layersValue[i - 1][j];
                            double sum = 0;
                            for (int k = 0; k < deltas.Length; k++)
                                sum += deltas[k] * network.weights[i][k][j];
                            nextdeltas[j] = der * sum;
                            for (int k = 0; k < deltas.Length; k++)
                                network.weights[i][k][j] += network.learningRate * deltas[k] * network.layersValue[i - 1][j];
                        }
                    }
                    deltas = new double[nextdeltas.Length - 1];
                    for (int j = 0; j < nextdeltas.Length - 1; j++)
                        deltas[j] = nextdeltas[j];
                }

                //input слой

                if (parallel)
                {
                    System.Threading.Tasks.Parallel.For(0, network.weights[0][0].Length, (j) =>
                    {
                        for (int k = 0; k < deltas.Length; k++)
                        {
                            network.weights[0][k][j] += network.learningRate * deltas[k] * network.inputs[j];
                        }
                    });
                }
                else
                {
                    for (int j = 0; j < network.weights[0][0].Length; j++)
                    {
                        for (int k = 0; k < deltas.Length; k++)
                        {
                            network.weights[0][k][j] += network.learningRate * deltas[k] * network.inputs[j];
                        }
                    }
                }
                ++iters;
            }
            return iters;
        }



        public override double TrainOnDataSet(SamplesSet samplesSet, int epochs_count, double acceptable_erorr, bool parallel = true)
        {
            int epoch_to_run = 0;
            double alpha = 0.15;
            double[][][] delta_weights;

            delta_weights = new double[network.weights.Length][][];
            if (parallel)
            {
                System.Threading.Tasks.Parallel.For(0, network.weights.Length, (i) =>
                {
                    delta_weights[i] = new double[network.weights[i].Length][];
                    for (int j = 0; j < network.weights[i].Length; j++)
                    {
                        delta_weights[i][j] = new double[network.weights[i][j].Length];
                    }
                });
            }
            else
            {
                for (int i = 0; i < network.weights.Length; i++)
                {
                    delta_weights[i] = new double[network.weights[i].Length][];
                    for (int j = 0; j < network.weights[i].Length; j++)
                    {
                        delta_weights[i][j] = new double[network.weights[i][j].Length];
                    }
                }
            }

            stopWatch.Restart();

            double error = double.PositiveInfinity;

            while (epoch_to_run < epochs_count && error > acceptable_erorr)
            {
                epoch_to_run++;
                for (int u = 0; u < samplesSet.Count; u++)
                {
                    samplesSet[u].output = network.Compute(samplesSet[u].input);
                    samplesSet[u].processOutput();
                    double[] deltas = new double[network.outputs.Length];

                    //идем в обратную сторону
                    if (parallel)
                    {
                        System.Threading.Tasks.Parallel.For(0, network.outputs.Length, (i) =>
                        {
                            double der = (1 - network.outputs[i]) * network.outputs[i];
                            deltas[i] = ((i == (int)samplesSet[u].actualClass ? 1 : 0) - network.outputs[i]) * der;
                        });
                    }
                    else
                    {
                        for (int i = 0; i < network.outputs.Length; i++)
                        {
                            double der = (1 - network.outputs[i]) * network.outputs[i];
                            deltas[i] = ((i == (int)samplesSet[u].actualClass ? 1 : 0) - network.outputs[i]) * der;
                        }
                    }

                    //A - слои

                    for (int i = network.weights.Length - 1; i >= 1; i--)
                    {
                        double[] nextdeltas = new double[network.weights[i][0].Length];
                        if (parallel)
                        {
                            System.Threading.Tasks.Parallel.For(0, network.weights[i][0].Length, (j) =>
                            {
                                double der = (1 - network.layersValue[i - 1][j]) * network.layersValue[i - 1][j];
                                double sum = 0;
                                for (int k = 0; k < deltas.Length; k++)
                                    sum += deltas[k] * network.weights[i][k][j];
                                nextdeltas[j] = der * sum;
                                for (int k = 0; k < deltas.Length; k++)
                                {
                                    delta_weights[i][k][j] = network.learningRate * deltas[k] * network.layersValue[i - 1][j] + alpha * delta_weights[i][k][j];
                                    network.weights[i][k][j] += delta_weights[i][k][j];
                                }
                            });
                        }
                        else
                        {
                            for (int j = 0; j < network.weights[i][0].Length; j++)
                            {
                                double der = (1 - network.layersValue[i - 1][j]) * network.layersValue[i - 1][j];
                                double sum = 0;
                                for (int k = 0; k < deltas.Length; k++)
                                    sum += deltas[k] * network.weights[i][k][j];
                                nextdeltas[j] = der * sum;
                                for (int k = 0; k < deltas.Length; k++)
                                {
                                    delta_weights[i][k][j] = network.learningRate * deltas[k] * network.layersValue[i - 1][j] + alpha * delta_weights[i][k][j];
                                    network.weights[i][k][j] += delta_weights[i][k][j];
                                }
                            }
                        }
                        deltas = new double[nextdeltas.Length-1];
                        for (int j = 0; j < nextdeltas.Length-1; j++)
                            deltas[j] = nextdeltas[j];
                    }

                    //input слой

                    if (parallel)
                    {
                        System.Threading.Tasks.Parallel.For(0, network.weights[0][0].Length, (j) =>
                        {
                            for (int k = 0; k < deltas.Length; k++)
                            {
                                delta_weights[0][k][j] = network.learningRate * deltas[k] * network.inputs[j] + alpha * delta_weights[0][k][j];
                                network.weights[0][k][j] += delta_weights[0][k][j];
                            }
                        });
                    }
                    else
                    {
                        for (int j = 0; j < network.weights[0][0].Length; j++)
                        {
                            for (int k = 0; k < deltas.Length; k++)
                            {
                                delta_weights[0][k][j] = network.learningRate * deltas[k] * network.inputs[j] + alpha * delta_weights[0][k][j];
                                network.weights[0][k][j] += delta_weights[0][k][j];
                            }
                        }
                    }
                }

                error = samplesSet[samplesSet.Count - 1].EstimatedError();

                updateDelegate((epoch_to_run * 1.0) / epochs_count, error, stopWatch.Elapsed);
            }

            updateDelegate(1.0, error, stopWatch.Elapsed);

            stopWatch.Stop();

            return error;
        }

        public override FigureType Predict(Sample sample)
        {
            sample.output = network.Compute(sample.input);
            sample.processOutput();
            return sample.recognizedClass;
        }

        public override double TestOnDataSet(SamplesSet testSet)
        {
            double correct = 0.0;
            for (int i = 0; i < testSet.Count; ++i)
            {
                testSet[i].output = network.Compute(testSet[i].input);
                testSet[i].processOutput();
                if (testSet[i].actualClass == testSet[i].recognizedClass) correct += 1;
            }
            return correct / testSet.Count;
        }

        public override double[] getOutput()
        {
            return network.outputs;
        }
    }
}
