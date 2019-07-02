//#define CONST_FIRST_PATRICLE // Для синхронной работы кластеров

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        static uint diameterBlackCircle;                      // Диаметр окружности
        static uint radiusBlackCircle;                             // Радиус окружности
        static uint dotsParam;                                  // Параметр задаваемый пользователем влияющий на количество точек на окружности 
        static uint particlesNum;                   // Количество точек на кольце
        static uint firstBlackCirclePositionY = 30;                 // Начальные точки отрисовки
        static uint firstBlackCirclePositionX = 270;                // Начальная точка отрисовки
        static uint diameterParticle;                          // Диаметр частицы
        static uint chainSize;                                  // Количество колец по оси, задаваемое пользователем
        static uint clustersLength ;                             // Длина кластера, задаваемая пользователем
        static uint ringsNum;               // Количество колец
        static Random rand = new Random();                          // Генератор случайных чисел
        static uint firstClusterParticle;                           // Начальное положение частицы
        uint indentFirstLastCenter;               // Отступ между центрами колец
        uint mutualCount ;         // Количество смежных точек
        uint firstBlackCircleCenterX ; // параметр указывающий на центр окружности по оси x
        uint firstBlackCircleCenterY ; // параметр указывающий на центр окружности по оси y
        uint indentFirstBlackCircleCenterX;             // параметр для изменения центральной точки кольца (используется для перехода на следующее кольцо) по оси x
        uint indentFirstBlackCircleCenterY;             // параметр для изменения центральной точки кольца (используется для перехода на следующее кольцо)по оси y
        double angleFi;  // угол на который нужно сдвинуться, чтобы нарисовать следующую точку на кольце
        Pen blackPen = new Pen(Brushes.Black, 3);       // Кисть для отрисовки
        Pen bluePen = new Pen(Brushes.Blue, 3);         // Задаем начальные параметры для отображения точек на кольцах
        bool startButtonClicked = false;

        // Класс, описывающий параметры частицы
        class particle
        {
            public float x = 0;
            public float y = 0;
            public Brush brush = Brushes.White;
            public Pen pen = Pens.Gray;
            public bool mutual = false; // Смежная частица или нет
        }
        static particle[,] particlesArray;// = new particle[ringsNum, particlesNum]; // Описание параметров всех частиц

        // Класс, описывающий состояние кольца
        class ringStatus
        {
            public uint previousFirstParticle;      // Последняя частица кластера
            public Queue<uint> coloredParticles;    // Перечень частиц, принадлежащих кластеру
            public bool stopped = false;            // Кластер данного кольца находится в ожидании освобождения смежной частицы или нет
            //public bool processed = false;          // Обработано ли кольцо

            public ringStatus()  // Конструктор
            {
#if CONST_FIRST_PATRICLE
                firstClusterParticle = 0;
#else
                firstClusterParticle = (uint)rand.Next() % particlesNum;
#endif
                previousFirstParticle = firstClusterParticle;
                // Инициализация списка
                uint currentNum = firstClusterParticle;
                coloredParticles = new Queue<uint>();
                for (uint i = 0; i < clustersLength; ++i)
                {
                    if (currentNum + 1 > particlesNum - 1)
                    {
                        coloredParticles.Enqueue(currentNum);
                        currentNum = 0;
                    }
                    else
                    {
                        coloredParticles.Enqueue(currentNum);
                        ++currentNum;
                    }
                }
            }

            public void update() // Сдвигает на 1 перечень частиц, принадлежащих кластеру 
            {
                previousFirstParticle = coloredParticles.First();
                uint lastElement = coloredParticles.ElementAt((int)(clustersLength - 1)); // 
                if (lastElement + 1 > particlesNum - 1)
                {
                    coloredParticles.Enqueue(0);
                }
                else
                {
                    coloredParticles.Enqueue(lastElement + 1);
                }
                coloredParticles.Dequeue();
            }
        }
        static ringStatus[] ringsStatuses;// = new ringStatus[ringsNum]; // Описание всех колец

        // Класс, описывающий смежную частицу, находящуюся по периметру
        public class mutualTor
        {
            public float pointX1 = 0;
            public float pointY1 = 0;
            public float pointX2 = 0;
            public float pointY2 = 0;
            public bool locked = false;
        }

        // Класс, описывающий смежную частицу, находящуюся внутри колец
        class mutualDuplicate
        {
            public float pointX = 0;
            public float pointY = 0;
            public bool locked = false;
        }

        // Класс, описывающий все смежные частицы
        class mutualsStatuses
        {
            public mutualTor[] mutualsTor;              // Все смежные частицы, находящиеся по периметру
            public mutualDuplicate[] mutualsDuplicate;  // Все смежные частицы, находящиеся внутри колец

            public mutualsStatuses() // Конструктор
            {
                mutualsTor = new mutualTor[2 * chainSize];
                mutualsDuplicate = new mutualDuplicate[2 * ((chainSize - 1) * chainSize)];
            }
        }
        static mutualsStatuses mutualsStatusesArray; // Описание всех смежных частиц

        /*---------------------------------------------------------------------------------------------------------------------------*/

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e) // Отрисовка окна
        {
            indentFirstBlackCircleCenterX = firstBlackCircleCenterX;
            indentFirstBlackCircleCenterY = firstBlackCircleCenterY;
            for (uint x = firstBlackCirclePositionX; x < indentFirstLastCenter + firstBlackCirclePositionX; x += diameterBlackCircle) // Сдвиг по оси x
            {
                for (uint y = firstBlackCirclePositionY; y < indentFirstLastCenter + firstBlackCirclePositionY; y += diameterBlackCircle) // Сдвиг по оси y
                {
                    // Отрисовка кольца
                    e.Graphics.DrawEllipse(blackPen, x, y, diameterBlackCircle, diameterBlackCircle);
                    indentFirstBlackCircleCenterX += diameterBlackCircle;
                }
                indentFirstBlackCircleCenterX = firstBlackCirclePositionX + radiusBlackCircle - 5;
                indentFirstBlackCircleCenterY += diameterBlackCircle;
            }
            if (startButtonClicked)
            {
                for (uint i = 0; i < ringsNum; ++i)
                {
                    for (uint j = 0; j < particlesNum; ++j)
                    {
                        // Отрисовка частицы
                        e.Graphics.FillEllipse(particlesArray[i, j].brush, particlesArray[i, j].x, particlesArray[i, j].y, diameterParticle, diameterParticle);
                        e.Graphics.DrawEllipse(particlesArray[i, j].pen, particlesArray[i, j].x, particlesArray[i, j].y, diameterParticle, diameterParticle);
                    }
                }
            }
        }

        private void setMutual(uint i, uint j) // Задает цвет для свободной смежной частицы и определяет ее как смежную
        {
            particlesArray[i, j].mutual = true;
            particlesArray[i, j].brush = Brushes.LightBlue;
            particlesArray[i, j].pen = Pens.LightBlue;
        }

        private void freeParticle(uint i, uint j) // Задает цвет для свободной частицы
        {
            particlesArray[i, j].brush = Brushes.White;
            particlesArray[i, j].pen = Pens.Gray;
        }

        private void setPatricle(uint i, uint j) // Задает цвет для частицы, принадлежащей кластеру
        {
            particlesArray[i, j].brush = Brushes.Green;
            particlesArray[i, j].pen = Pens.Green;
        }

        private void lockMutualParticle(uint i, uint j) // Задает цвет для занятой смежной частицы
        {
            particlesArray[i, j].brush = Brushes.Red;
            particlesArray[i, j].pen = Pens.Red;
        }

        private void setWaitPatricles(uint i, uint firstJ, uint sizeJ) // Задает цвет для частиц, которые ожидают освобождения смежной частицы
        {
            uint currentJ = firstJ;
            for (uint j = 0; j < sizeJ; ++j)
            {
                particlesArray[i, currentJ].brush = Brushes.Yellow;
                particlesArray[i, currentJ].pen = Pens.Yellow;
                if (currentJ + 1 > particlesNum - 1)
                    currentJ = 0;
                else
                    ++currentJ;
            }
        }

        // Обновление состояний при одновременной попытке занять смежную частицу
        private void setTwoRingsWhileParallelProcessing(ref uint ringNum, ref uint ringNum2, ref bool[] needUpdateFlags, ref bool[] stopFlags)
        {
            needUpdateFlags[ringNum] = true;
            stopFlags[ringNum] = false;
            ringsStatuses[ringNum].stopped = false;

            needUpdateFlags[ringNum2] = false;
            stopFlags[ringNum2] = true;
            ringsStatuses[ringNum2].stopped = true;
        }

        // Выбор одного из двух колец при одновременной попытке занять смежную частицу
        private void parallelProcess(ref uint ringNum, ref uint ringNum2, ref bool[] needUpdateFlags, ref bool[] stopFlags)
        {
            uint randomPass = (uint)rand.Next() % 1;
            if (randomPass == 0)
                setTwoRingsWhileParallelProcessing(ref ringNum, ref ringNum2, ref needUpdateFlags, ref stopFlags);
            else
                setTwoRingsWhileParallelProcessing(ref ringNum2, ref ringNum, ref needUpdateFlags, ref stopFlags);
        }

        // Совпадают ли координаты частиц
        static private bool foundTor(uint ringNum, uint nextElement, mutualTor mutualTor)
        {
            return (particlesArray[ringNum, nextElement].x == mutualTor.pointX1 &&
                    particlesArray[ringNum, nextElement].y == mutualTor.pointY1) ||
                    (particlesArray[ringNum, nextElement].x == mutualTor.pointX2 &&
                    particlesArray[ringNum, nextElement].y == mutualTor.pointY2);
        }

        // Совпадают ли координаты частиц
        private bool foundDuplicate(uint ringNum, uint nextElement, mutualDuplicate mutualDuplicate)
        {
            return (particlesArray[ringNum, nextElement].x == mutualDuplicate.pointX &&
                    particlesArray[ringNum, nextElement].y == mutualDuplicate.pointY);
        }

        // Основной анализ поведения кольчуги
        private void ringsAnalize(ref bool[] needUpdateFlags, ref bool[] stopFlags)
        {
            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                uint lastElement = ringsStatuses[ringNum].coloredParticles.ElementAt((int)(clustersLength - 1));
                uint nextElement;
                if (lastElement + 1 > particlesNum - 1)
                    nextElement = 0;
                else
                    nextElement = lastElement + 1;
                // Попытка завладеть частицей
                if (!ringsStatuses[ringNum].stopped && (particlesArray[ringNum, nextElement].mutual))
                {
                    // Поиск точки в массиве смежных частиц по периметру
                    foreach (mutualTor mutualTor in mutualsStatusesArray.mutualsTor)
                    {
                        if (foundTor(ringNum, nextElement, mutualTor))
                        {
                            if (!mutualTor.locked)
                            {
                                mutualTor.locked = true;
                                needUpdateFlags[ringNum] = true;
                                break;
                            }
                            else
                            {
                                ringsStatuses[ringNum].stopped = true;
                                stopFlags[ringNum] = true;
                                break;
                            }
                        }
                    }
                    // Поиск точки в массиве смежных частиц внутри колец
                    foreach (mutualDuplicate mutualDuplicate in mutualsStatusesArray.mutualsDuplicate)
                    {
                        if (foundDuplicate(ringNum, nextElement, mutualDuplicate))
                        {
                            if (!mutualDuplicate.locked)
                            {
                                mutualDuplicate.locked = true;
                                needUpdateFlags[ringNum] = true;
                                break;
                            }
                            else
                            {
                                ringsStatuses[ringNum].stopped = true;
                                stopFlags[ringNum] = true;
                                break;
                            }
                        }
                    }
                }
                else if (ringsStatuses[ringNum].stopped) // Попытка выхода кластера из ожидания
                {
                    foreach (mutualTor mutualTor in mutualsStatusesArray.mutualsTor)
                    {
                        if (foundTor(ringNum, nextElement, mutualTor))
                        {
                            if (!mutualTor.locked)
                            {
                                ringsStatuses[ringNum].stopped = false;
                                stopFlags[ringNum] = false;
                                needUpdateFlags[ringNum] = true;
                                mutualTor.locked = true;
                                break;
                            }
                        }
                    }
                    foreach (mutualDuplicate mutualDuplicate in mutualsStatusesArray.mutualsDuplicate)
                    {
                        if (foundDuplicate(ringNum, nextElement, mutualDuplicate))
                        {
                            if (!mutualDuplicate.locked)
                            {
                                ringsStatuses[ringNum].stopped = false;
                                stopFlags[ringNum] = false;
                                needUpdateFlags[ringNum] = true;
                                mutualDuplicate.locked = true;
                                break;
                            }
                        }
                    }
                }
                // В остальных случаях сдвигаем кластер
                else if (!ringsStatuses[ringNum].stopped)
                {
                    needUpdateFlags[ringNum] = true;
                }
            }
        }

        /*!
         * Обновление состояния смежной частицы. Внешняя смежная частица
         * ringNum - Текущее кольцо
         * particleNum - Частица текущего кольца
         * neibourRingNum - Смежное кольцо
         * neibourParticleNum - Частица смежного кольца
         * torNum - Индекс смежной частицы в массиве mutualsStatusesArray.mutualsDuplicate
         */
        private void updateRingsStatusesTor(uint ringNum, uint particleNum, uint neibourRingNum, uint neibourParticleNum, uint torNum)
        {
            if (mutualsStatusesArray.mutualsTor[torNum].locked)
            {
                if (!(ringsStatuses[neibourRingNum].stopped &&
                    ringsStatuses[neibourRingNum].coloredParticles.Contains(neibourParticleNum)))
                {
                    mutualsStatusesArray.mutualsTor[torNum].locked = false;
                }
            }
        }

        /*!
         * Обновление состояния смежной частицы. Внутренняя смежная частица
         * ringNum - Текущее кольцо
         * particleNum - Частица текущего кольца
         * neibourRingNum - Смежное кольцо
         * neibourParticleNum - Частица смежного кольца
         * duplicateNum - Индекс смежной частицы в массиве mutualsStatusesArray.mutualsDuplicate
         */
        private void updateRingsStatusesDuplicate(uint ringNum, uint particleNum, uint neibourRingNum, uint neibourParticleNum, uint duplicateNum)
        {
            if (mutualsStatusesArray.mutualsDuplicate[duplicateNum].locked)
            {
                if (!(ringsStatuses[neibourRingNum].stopped &&
                    ringsStatuses[neibourRingNum].coloredParticles.Contains(neibourParticleNum)))
                {
                    mutualsStatusesArray.mutualsDuplicate[duplicateNum].locked = false;
                }
            }
        }

        /*!
         * Освобождение последнего элемента перед сдвигом кластера, сдвиг кластеров
         * needUpdateFlags - Если необходимо обновить кольцо - true
         * stopFlags - Если кольцо остановлено - true
         */
        private void updateRingsStatuses(ref bool[] needUpdateFlags, ref bool[] stopFlags)
        {
            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                if (needUpdateFlags[ringNum] && !stopFlags[ringNum])
                {
                    // Освобождение последнего элемента перед сдвигом кластера
                    uint particleToFreeNum = ringsStatuses[ringNum].coloredParticles.First();
                    if (particlesArray[ringNum, particleToFreeNum].mutual && !ringsStatuses[ringNum].stopped)
                    {
                        // Частица справа
                        if (particleToFreeNum == 0) 
                        {
                            if (ringNum >= (chainSize * chainSize) - chainSize)
                                updateRingsStatusesTor(ringNum, 0, ringNum % chainSize, dotsParam * 2, chainSize + ringNum % (chainSize * (chainSize - 1)));
                            if (ringNum < (chainSize * chainSize) - chainSize)
                                updateRingsStatusesDuplicate(ringNum, 0, ringNum + chainSize, dotsParam * 2, (ringNum / (chainSize * (chainSize - 1)) + ringNum * (chainSize - 1) + (ringNum / chainSize)) % (chainSize * (chainSize - 1)));
                        }
                        // Частица снизу
                        if (particleToFreeNum == dotsParam)
                        {
                            if (ringNum % chainSize == chainSize - 1)
                                updateRingsStatusesTor(ringNum, dotsParam, ringNum - chainSize + 1, dotsParam * 3, ringNum / chainSize + ringNum % chainSize - (chainSize - 1));
                            if (ringNum % chainSize < (chainSize - 1))
                                updateRingsStatusesDuplicate(ringNum, dotsParam, ringNum + 1, dotsParam * 3, ((ringNum % chainSize) * chainSize + (ringNum / chainSize)) + chainSize * (chainSize - 1));
                        }
                        // Частица слева
                        if (particleToFreeNum == dotsParam * 2)
                        {
                            if (ringNum < chainSize)
                                updateRingsStatusesTor(ringNum, 2 * dotsParam, chainSize * chainSize - chainSize + ringNum, 0, ringNum + chainSize);
                            if (ringNum > chainSize - 1)
                                updateRingsStatusesDuplicate(ringNum, 2 * dotsParam, ringNum - chainSize, 0, ((ringNum - chainSize) / (chainSize * (chainSize - 1)) + (ringNum - chainSize) * (chainSize - 1) +
                                                        ((ringNum - chainSize) / chainSize)) % (chainSize * (chainSize - 1)));
                        }
                        // Частица сверху
                        if (particleToFreeNum == dotsParam * 3)
                        {
                            if (ringNum % chainSize == 0)
                                updateRingsStatusesTor(ringNum, 3 * dotsParam, ringNum + chainSize - 1, dotsParam, ringNum / chainSize);
                            if (ringNum % chainSize != 0)
                                updateRingsStatusesDuplicate(ringNum, 3 * dotsParam, ringNum - 1, dotsParam, (((ringNum - 1) % chainSize) * chainSize + ((ringNum - 1) / chainSize)) + chainSize * (chainSize - 1));
                        }
                    }
                    ringsStatuses[ringNum].update(); // Сдвиг кластера
                }
            }
        }

        /*!
         * Обработка ситуации: при одновременной попытке занять смежную частицу. Внешняя смежная частица
         * ringNum - Текущее кольцо
         * needUpdateFlags - Если необходимо обновить кольцо - true
         * stopFlags - Если кольцо остановлено - true
         * particleNum - Частица текущего кольца
         * neibourRingNum - Смежное кольцо
         * currentNumCompare - Частица текущего кольца, предшествующая смежной
         * neibourNumCompare - Частица смежного кольца, предшествующая смежной
         * compareFirstCoords - true если необходимо сравнивать верхние/левые внешние частицы, false нижние/правые
         */
        private void simultaneousLockTor(uint ringNum, ref bool[] needUpdateFlags, ref bool[] stopFlags, uint particleNum, 
                                         uint neibourRingNum, uint currentNumCompare, uint neibourNumCompare, bool compareFirstCoords)
        {
            float compareCoordX;
            float compareCoordY;
            foreach (mutualTor mutualTor in mutualsStatusesArray.mutualsTor) // Поиск частицы
            {
                if (compareFirstCoords)
                {
                    compareCoordX = mutualTor.pointX1;
                    compareCoordY = mutualTor.pointY1;
                }
                else
                {
                    compareCoordX = mutualTor.pointX2;
                    compareCoordY = mutualTor.pointY2;
                }
                if (particlesArray[ringNum, particleNum].x == compareCoordX && // Если частица найдена
                    particlesArray[ringNum, particleNum].y == compareCoordY)
                {
                    if (mutualTor.locked) // Если частица занята
                    {
                        // Условие одновременной попытки занять частицу
                        if ((ringsStatuses[neibourRingNum].coloredParticles.ElementAt((int)(clustersLength - 1)) == neibourNumCompare) && 
                            (ringsStatuses[ringNum].coloredParticles.ElementAt((int)(clustersLength - 1)) == currentNumCompare))
                        {
                            parallelProcess(ref ringNum, ref neibourRingNum, ref needUpdateFlags, ref stopFlags); // Обработка ситуации
                            break;
                        }
                    }
                }
            }
        }

        /*!
         * Обработка ситуации: при одновременной попытке занять смежную частицу. Внутренняя смежная частица
         * ringNum - Текущее кольцо
         * needUpdateFlags - Если необходимо обновить кольцо - true
         * stopFlags - Если кольцо остановлено - true
         * particleNum - Частица текущего кольца
         * neibourRingNum - Смежное кольцо
         * currentNumCompare - Частица текущего кольца, предшествующая смежной
         * neibourNumCompare - Частица смежного кольца, предшествующая смежной
         */
        private void simultaneousLockDuplicate(uint ringNum, ref bool[] needUpdateFlags, ref bool[] stopFlags, uint particleNum,
                                               uint neibourRingNum, uint currentNumCompare, uint neibourNumCompare)
        {
            foreach (mutualDuplicate mutualDuplicate in mutualsStatusesArray.mutualsDuplicate) // Поиск частицы
            {
                if (particlesArray[ringNum, particleNum].x == mutualDuplicate.pointX && // Если частица найдена
                    particlesArray[ringNum, particleNum].y == mutualDuplicate.pointY)
                {
                    if (mutualDuplicate.locked) // Если частица занята
                    {
                        // Условие одновременной попытки занять частицу
                        if ((ringsStatuses[neibourRingNum].coloredParticles.ElementAt((int)(clustersLength - 1)) == neibourNumCompare) &&
                            (ringsStatuses[ringNum].coloredParticles.ElementAt((int)(clustersLength - 1)) == currentNumCompare))
                        {
                            parallelProcess(ref ringNum, ref neibourRingNum, ref needUpdateFlags, ref stopFlags); // Обработка ситуации
                            break;
                        }
                    }
                }
            }
        }

        // Обработка ситуации: при одновременной попытке занять смежную частицу
        private void simultaneousLock(ref bool[] needUpdateFlags, ref bool[] stopFlags)
        {
            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                // Частица справа
                simultaneousLockTor(ringNum, ref needUpdateFlags, ref stopFlags, 0, ringNum % chainSize, dotsParam * 4 - 1, dotsParam * 2 - 1, false);
                simultaneousLockDuplicate(ringNum, ref needUpdateFlags, ref stopFlags, 0, ringNum + chainSize, dotsParam * 4 - 1, dotsParam * 2 - 1); 
                // Частица снизу
                simultaneousLockTor(ringNum, ref needUpdateFlags, ref stopFlags, dotsParam, ringNum - chainSize + 1, dotsParam - 1, dotsParam * 3 - 1, false);
                simultaneousLockDuplicate(ringNum, ref needUpdateFlags, ref stopFlags, dotsParam, ringNum + 1, dotsParam - 1, dotsParam * 3 - 1);
                // Частица слева
                simultaneousLockTor(ringNum, ref needUpdateFlags, ref stopFlags, 2 * dotsParam, chainSize * chainSize - chainSize + ringNum, dotsParam * 2 - 1, dotsParam * 4 - 1, true);
                simultaneousLockDuplicate(ringNum, ref needUpdateFlags, ref stopFlags, 2 * dotsParam, ringNum - chainSize, dotsParam * 2 - 1, dotsParam * 4 - 1);
                // Частица сверху
                simultaneousLockTor(ringNum, ref needUpdateFlags, ref stopFlags, 3 * dotsParam, ringNum + chainSize - 1, dotsParam * 3 - 1, dotsParam - 1, true);
                simultaneousLockDuplicate(ringNum, ref needUpdateFlags, ref stopFlags, 3 * dotsParam, ringNum - 1, dotsParam * 3 - 1, dotsParam - 1);
            }
            // Сдвиг кластеров
            updateRingsStatuses(ref needUpdateFlags, ref stopFlags);
        }
        
        /*
         * Обработка ситуации: если кольцо в ожидании, но частица свободна (обработалась позже). Для внешних точек
         * ringNum - текущее кольцо
         * needUpdateFlags - Если необходимо обновить кольцо - true
         * stopFlags - Если кольцо остановлено - true
         * particleNum - Номер смежной частицы в текущем кольце
         * neibourRingNum - Номер смежного кольца
         * previousFirstParticleNum - Номер смежной частицы в смежном кольце
         * compareFirstCoords - true если необходимо сравнивать верхние/левые внешние частицы, false нижние/правые
         */
        private void postProcessingRingsTor(uint ringNum, ref bool[] needUpdateFlags, ref bool[] stopFlags, uint particleNum,
                                            uint neibourRingNum, uint previousFirstParticleNum, bool compareFirstCoords)
        {
            float compareCoordX;
            float compareCoordY;
            foreach (mutualTor mutualTor in mutualsStatusesArray.mutualsTor) // Поиск конкретной смежной частицы
            {
                if (compareFirstCoords) // Поиск по необходимым координатам
                {
                    compareCoordX = mutualTor.pointX1;
                    compareCoordY = mutualTor.pointY1;
                }
                else
                {
                    compareCoordX = mutualTor.pointX2;
                    compareCoordY = mutualTor.pointY2;
                }
                if ((particlesArray[ringNum, particleNum].x == compareCoordX && // Если смежная частица найдена (по координатам)
                     particlesArray[ringNum, particleNum].y == compareCoordY))
                {
                    if (!mutualTor.locked) // Если смежная частица не занята
                    {
                        // Если предыдущая последняя частица кластера была на смежной частице
                        if (ringsStatuses[neibourRingNum].previousFirstParticle == previousFirstParticleNum) 
                        {
                            needUpdateFlags[ringNum] = true;
                            ringsStatuses[ringNum].stopped = false;
                            mutualTor.locked = true;
                            break;
                        }
                    }
                }
            }
        }

        /*
         * Обработка ситуации: если кольцо в ожидании, но частица свободна (обработалась позже). Для внутренних точек
         * ringNum - текущее кольцо
         * needUpdateFlags - Если необходимо обновить кольцо - true
         * stopFlags - Если кольцо остановлено - true
         * particleNum - Номер смежной частицы в текущем кольце
         * neibourRingNum - Номер смежного кольца
         * previousFirstParticleNum - Номер смежной частицы в смежном кольце
         */
        private void postProcessingRingsDuplicate(uint ringNum, ref bool[] needUpdateFlags, ref bool[] stopFlags, uint particleNum,
                                                  uint neibourRingNum, uint previousFirstParticleNum)
        {
            foreach (mutualDuplicate mutualDuplicate in mutualsStatusesArray.mutualsDuplicate) // Поиск конкретной смежной частицы
            {
                if ((particlesArray[ringNum, particleNum].x == mutualDuplicate.pointX && // Если смежная частица найдена (по координатам)
                     particlesArray[ringNum, particleNum].y == mutualDuplicate.pointY))
                {
                    if (!mutualDuplicate.locked) // Если смежная частица не занята
                    {
                        // Если предыдущая последняя частица кластера была на смежной частице
                        if (ringsStatuses[neibourRingNum].previousFirstParticle == previousFirstParticleNum) 
                        {
                            needUpdateFlags[ringNum] = true;
                            ringsStatuses[ringNum].stopped = false;
                            mutualDuplicate.locked = true;
                            break;
                        }
                    }
                }
            }
        }
        
        // Обработка ситуации: если кольцо в ожидании, но частица свободна (обработалась позже)
        private void postProcessingRings() 
        {
            bool[] needUpdateFlags = new bool[ringsNum]; // Если необходимо обновить кольцо - true
            bool[] stopFlags = new bool[ringsNum];       // Если кольцо остановлено - true
            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                needUpdateFlags[ringNum] = false;
                stopFlags[ringNum] = false;
            }
            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                if (ringsStatuses[ringNum].stopped)
                {
                    uint lastElement = ringsStatuses[ringNum].coloredParticles.ElementAt((int)(clustersLength - 1));
                    uint nextElement;
                    if (lastElement + 1 > particlesNum - 1)
                        nextElement = 0;
                    else
                        nextElement = lastElement + 1;
                    // Частица справа
                    if (nextElement == 0)
                    {
                        // Поиск точки в массиве смежных частиц по периметру
                        postProcessingRingsTor(ringNum, ref needUpdateFlags, ref stopFlags, 0, ringNum % chainSize, nextElement + 2 * dotsParam, false);
                        // Поиск точки в массиве смежных частиц внутри колец
                        postProcessingRingsDuplicate(ringNum, ref needUpdateFlags, ref stopFlags, 0, ringNum + chainSize, nextElement + 2 * dotsParam);
                    }

                    if (nextElement == dotsParam)
                    {
                        // Частица снизу
                        // Поиск точки в массиве смежных частиц по периметру
                        postProcessingRingsTor(ringNum, ref needUpdateFlags, ref stopFlags, dotsParam, ringNum - chainSize + 1, nextElement + 2 * dotsParam, false);
                        // Поиск точки в массиве смежных частиц внутри колец
                        postProcessingRingsDuplicate(ringNum, ref needUpdateFlags, ref stopFlags, dotsParam, ringNum + 1, nextElement + 2 * dotsParam);
                    }

                    if (nextElement == dotsParam * 2)
                    {
                        // Частица слева
                        // Поиск точки в массиве смежных частиц по периметру
                        postProcessingRingsTor(ringNum, ref needUpdateFlags, ref stopFlags, 2 * dotsParam, chainSize * chainSize - chainSize + ringNum, 0, true);
                        // Поиск точки в массиве смежных частиц внутри колец
                        postProcessingRingsDuplicate(ringNum, ref needUpdateFlags, ref stopFlags, 2 * dotsParam, ringNum - chainSize, 0);
                    }

                    if (nextElement == dotsParam * 3)
                    {
                        // Частица сверху
                        // Поиск точки в массиве смежных частиц по периметру
                        postProcessingRingsTor(ringNum, ref needUpdateFlags, ref stopFlags, 3 * dotsParam, ringNum + chainSize - 1, dotsParam, true);
                        // Поиск точки в массиве смежных частиц внутри колец
                        postProcessingRingsDuplicate(ringNum, ref needUpdateFlags, ref stopFlags, 3 * dotsParam, ringNum - 1, dotsParam);
                    }
                }
            }
            updateRingsStatuses(ref needUpdateFlags, ref stopFlags);
        }

        // Обновление состояния кольчуги (логическое)
        void updateChain() 
        {
            bool[] needUpdateFlags = new bool[ringsNum]; // Если необходимо обновить кольцо - true
            bool[] stopFlags = new bool[ringsNum];       // Если кольцо остановлено - true

            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                needUpdateFlags[ringNum] = false;
                stopFlags[ringNum] = false;
            }

            // Основной анализ поведения кольчуги
            ringsAnalize(ref needUpdateFlags, ref stopFlags);

            // Обработка ситуации: при одновременной попытке занять смежную частицу
            simultaneousLock(ref needUpdateFlags, ref stopFlags);

            // Обработка ситуации: если кольцо в ожидании, но частица свободна (обработалась позже)
            postProcessingRings();
        }

        // Задание цветовых параметров частиц перед отрисовкой
        void processParticles() 
        {
            // Цветовые параметры частиц внутри кластера
            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                if (!ringsStatuses[ringNum].stopped) // Если кольцо не в состоянии ожидания
                {
                    foreach (uint needToColorParticleNum in ringsStatuses[ringNum].coloredParticles)
                    {
                        if (!particlesArray[ringNum, needToColorParticleNum].mutual)
                            setPatricle(ringNum, needToColorParticleNum);
                        else // Если занятая смежная частица
                        {
                            foreach (mutualTor mutualTorPoint in mutualsStatusesArray.mutualsTor)
                            {
                                if (mutualTorPoint.locked)
                                    lockMutualParticle(ringNum, needToColorParticleNum);
                            }
                            foreach (mutualDuplicate mutualTorPoint in mutualsStatusesArray.mutualsDuplicate)
                            {
                                if (mutualTorPoint.locked)
                                    lockMutualParticle(ringNum, needToColorParticleNum);
                            }
                        }
                    }
                }
                else // Если кольцо в состоянии ожидания
                {
                    foreach (uint needToColorParticleNum in ringsStatuses[ringNum].coloredParticles)
                    {
                        if (!particlesArray[ringNum, needToColorParticleNum].mutual)
                            setWaitPatricles(ringNum, ringsStatuses[ringNum].coloredParticles.First(), clustersLength);
                        else // Если занятая смежная частица
                        {
                            foreach (mutualTor mutualTorPoint in mutualsStatusesArray.mutualsTor)
                            {
                                if (mutualTorPoint.locked)
                                    lockMutualParticle(ringNum, needToColorParticleNum);
                            }
                            foreach (mutualDuplicate mutualTorPoint in mutualsStatusesArray.mutualsDuplicate)
                            {
                                if (mutualTorPoint.locked)
                                    lockMutualParticle(ringNum, needToColorParticleNum);
                            }
                        }
                    }
                }
            }
            // Цветовые параметры частиц вне кластера
            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                for (uint particleNum = 0; particleNum < particlesNum; ++particleNum)
                {
                    if (!ringsStatuses[ringNum].coloredParticles.Contains(particleNum))
                    {
                        if (!particlesArray[ringNum, particleNum].mutual)
                            freeParticle(ringNum, particleNum);
                        else // Если свободная смежная частица
                        {
                            if (ringsStatuses[ringNum].previousFirstParticle == particleNum)
                                setMutual(ringNum, particleNum);
                            // Если частица по периметру
                            if (((ringNum % 3 == 0) && (particleNum == dotsParam * 3)) ||           // Если сверху
                                ((ringNum < chainSize) && (particleNum == dotsParam * 2)) ||        // Если слева
                                ((ringNum >= chainSize * (chainSize - 1)) && (particleNum == 0)) || // Если справа
                                ((ringNum % chainSize == 2) && (particleNum == dotsParam)))         // Если снизу
                            {
                                foreach (mutualTor mutualTorPoint in mutualsStatusesArray.mutualsTor)
                                {
                                    if (foundTor(ringNum, particleNum, mutualTorPoint))
                                    {
                                        if (!mutualTorPoint.locked)
                                        {
                                            setMutual(ringNum, particleNum);
                                            break;
                                        }
                                        else
                                        {
                                            lockMutualParticle(ringNum, particleNum);
                                            break;
                                        }
                                    }
                                }
                            }
                            else // Если частица внутри
                            {
                                foreach (mutualDuplicate mutualDuplicatePoint in mutualsStatusesArray.mutualsDuplicate)
                                {
                                    if (foundDuplicate(ringNum, particleNum, mutualDuplicatePoint))
                                    {
                                        if (!mutualDuplicatePoint.locked)
                                        {
                                            setMutual(ringNum, particleNum);
                                            break;
                                        }
                                        else
                                        {
                                            lockMutualParticle(ringNum, particleNum);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void initializeChain() // Определение координат частиц, заполнение соответствующих массивов
        {
            indentFirstBlackCircleCenterX = firstBlackCircleCenterX;
            indentFirstBlackCircleCenterY = firstBlackCircleCenterY;
            for (uint i = 0; i < ringsNum; ++i)
            {
                int jCount = 0;
                for (uint j = 0; j < particlesNum; ++j)
                {
                    particlesArray[i, j] = new particle();
                    particlesArray[i, j].x = indentFirstBlackCircleCenterX + (float)(radiusBlackCircle * Math.Cos(angleFi * jCount));
                    particlesArray[i, j].y = indentFirstBlackCircleCenterY + (float)(radiusBlackCircle * Math.Sin(angleFi * jCount));
                    jCount++;
                }
                indentFirstBlackCircleCenterY += diameterBlackCircle;
                if ((i + 1) % chainSize == 0)
                {
                    indentFirstBlackCircleCenterX += diameterBlackCircle;
                    indentFirstBlackCircleCenterY = firstBlackCircleCenterY;
                }
                for (uint j = 0; j < 4; ++j)
                {
                    setMutual(i, dotsParam * j); // Определить точку как смежную
                }
            }
            for (uint i = 0; i < ringsNum; ++i)
            {
                ringsStatuses[i] = new ringStatus();
            }
            // Указание координат частиц по периметру
            for (uint i = 0; i < chainSize; ++i)
            {
                mutualsStatusesArray.mutualsTor[i] = new mutualTor();
                mutualsStatusesArray.mutualsTor[i + chainSize] = new mutualTor();
                // Вертикальные частицы
                mutualsStatusesArray.mutualsTor[i].pointX1 = particlesArray[i * chainSize, dotsParam * 3].x;
                mutualsStatusesArray.mutualsTor[i].pointY1 = particlesArray[i * chainSize, dotsParam * 3].y;
                mutualsStatusesArray.mutualsTor[i].pointX2 = particlesArray[i * chainSize + (chainSize - 1), dotsParam].x;
                mutualsStatusesArray.mutualsTor[i].pointY2 = particlesArray[i * chainSize + (chainSize - 1), dotsParam].y;
                // Горизонтальные частицы
                mutualsStatusesArray.mutualsTor[i + chainSize].pointX1 = particlesArray[i, dotsParam * 2].x;
                mutualsStatusesArray.mutualsTor[i + chainSize].pointY1 = particlesArray[i, dotsParam * 2].y;
                mutualsStatusesArray.mutualsTor[i + chainSize].pointX2 = particlesArray[i + (chainSize - 1) * (chainSize), 0].x;
                mutualsStatusesArray.mutualsTor[i + chainSize].pointY2 = particlesArray[i + (chainSize - 1) * (chainSize), 0].y;
            }
            // Указание координат внутренних горизонтальных частиц
            uint currentMutual = 0;
            for (uint i = 0; i < chainSize; ++i)
            {
                for (uint j = 0; j < chainSize - 1; ++j)
                {
                    mutualsStatusesArray.mutualsDuplicate[currentMutual] = new mutualDuplicate();
                    mutualsStatusesArray.mutualsDuplicate[currentMutual].pointX = particlesArray[j * chainSize + i, 0].x;
                    mutualsStatusesArray.mutualsDuplicate[currentMutual].pointY = particlesArray[j * chainSize + i, 0].y;
                    ++currentMutual;
                }
            }
            // Указание координат внутренних вертикальных частиц
            for (uint i = 0; i < chainSize - 1; ++i)
            {
                for (uint j = 0; j < chainSize; ++j)
                {
                    mutualsStatusesArray.mutualsDuplicate[(chainSize * (chainSize - 1)) + i * chainSize + j] = new mutualDuplicate();
                    mutualsStatusesArray.mutualsDuplicate[(chainSize * (chainSize - 1)) + i * chainSize + j].pointX = particlesArray[j * chainSize + i, dotsParam].x;
                    mutualsStatusesArray.mutualsDuplicate[(chainSize * (chainSize - 1)) + i * chainSize + j].pointY = particlesArray[j * chainSize + i, dotsParam].y;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e) // Начальная инициализация формы
        {
            this.Paint += Form1_Paint;                   // Задание обработчика к событию формы Paint
        }

        private void timer1_Tick(object sender, EventArgs e) // Обратобка таймера
        {
            updateChain();      // Обновление состояния кольчуги
            processParticles(); // Задание цветовых параметров частиц перед отрисовкой
            this.Invalidate();  // Вызов отрисовки формы
        }

        private void setGlobalVariables() // Задать значения глобальным переменным
        {
            radiusBlackCircle = diameterBlackCircle / 2;    // Радиус окружности
            indentFirstLastCenter = chainSize * diameterBlackCircle;     // Отступ между центрами колец
            firstBlackCircleCenterX = firstBlackCirclePositionX + (radiusBlackCircle - diameterParticle / 2);// параметр указывающий на центр окружности по оси x
            firstBlackCircleCenterY = firstBlackCirclePositionY + (radiusBlackCircle - diameterParticle / 2);// параметр указывающий на центр окружности по оси y

            particlesNum = 4 * dotsParam;        // Количество точек на кольце
            ringsNum = chainSize * chainSize;    // Количество колец
            indentFirstLastCenter = chainSize * diameterBlackCircle;        // Отступ между центрами колец
            mutualCount = 2 * (((chainSize - 1) * chainSize) + chainSize);  // Количество смежных точек
            angleFi = 2 * (Math.PI) / particlesNum;  // угол на который нужно сдвинуться, чтобы нарисовать следующую точку на кольце
            particlesArray = new particle[ringsNum, particlesNum]; // Описание параметров всех частиц
            ringsStatuses = new ringStatus[ringsNum];       // Описание статусов колец
            mutualsStatusesArray = new mutualsStatuses();   // Описание всех смежных частиц
        }

        /*!
         *  ringNum     текущее кольцо
         *  torNum      индекс смежной частицы в массиве mutualsStatusesArray.mutualsTor
         *  neibourNum  номер кольца, смежного с текущим
         *  condValue1  номер смежной частицы в смежном кольце
         *  condValue2  номер смежной частицы в текущем кольце
         */
        private void initialAnalizeTor(uint ringNum, uint torNum, uint neibourNum, uint condValue1, uint condValue2)
        {
            if (!mutualsStatusesArray.mutualsTor[torNum].locked) // Если смежная частица не занята
                if ((ringsStatuses[neibourNum].coloredParticles.Contains(condValue1)) || // Если кластер смежного кольца занимает смежную частицу
                    (ringsStatuses[ringNum].coloredParticles.Contains(condValue2)))      // Если кластер текущего кольца занимает смежную частицу
                {
                    mutualsStatusesArray.mutualsTor[torNum].locked = true; // Пометить частицу как занятую
                }
        }

        /*!
         *  ringNum         текущее кольцо
         *  duplicateNum    индекс смежной частицы в массиве mutualsStatusesArray.mutualsDuplicate
         *  neibourNum      номер кольца, смежного с текущим
         *  condValue1      номер смежной частицы в смежном кольце
         *  condValue2      номер смежной частицы в текущем кольце
         */
        private void initialAnalizeDuplicate(uint ringNum, uint duplicateNum, uint neibourNum, uint condValue1, uint condValue2)
        {
            if (!mutualsStatusesArray.mutualsDuplicate[duplicateNum].locked) // Если смежная частица не занята
                if ((ringsStatuses[neibourNum].coloredParticles.Contains(condValue1)) || // Если кластер смежного кольца занимает смежную частицу
                    (ringsStatuses[ringNum].coloredParticles.Contains(condValue2)))      // Если кластер текущего кольца занимает смежную частицу
                {
                    mutualsStatusesArray.mutualsDuplicate[duplicateNum].locked = true; // Пометить частицу как занятую
                }
        }

        private void initialAnalize() // Определяет состояния смежных частиц
        {
            for (uint ringNum = 0; ringNum < ringsNum; ++ringNum)
            {
                // Частица справа
                //Поиск точки в массиве смежных частиц по периметру
                if (ringNum >= (chainSize * chainSize) - chainSize)
                    initialAnalizeTor(ringNum, chainSize + ringNum % (chainSize * (chainSize - 1)), ringNum % chainSize, dotsParam * 2, 0);
                // Поиск точки в массиве смежных частиц внутри колец //todo: add else if
                else if (ringNum < (chainSize * chainSize) - chainSize)
                    initialAnalizeDuplicate(ringNum, (ringNum / (chainSize * (chainSize - 1)) + ringNum * (chainSize - 1) + (ringNum / chainSize)) % (chainSize * (chainSize - 1)),
                                            ringNum + chainSize, dotsParam * 2, 0);
                // Частица снизу
                // Поиск точки в массиве смежных частиц по периметру
                else if(ringNum % chainSize == chainSize - 1)
                    initialAnalizeTor(ringNum, ringNum / chainSize + ringNum % chainSize - (chainSize - 1), ringNum - chainSize + 1, dotsParam * 3, dotsParam);
                // Поиск точки в массиве смежных частиц внутри колец
                else if(ringNum % chainSize < (chainSize - 1))
                    initialAnalizeDuplicate(ringNum, ((ringNum % chainSize) * chainSize + (ringNum / chainSize)) + chainSize * (chainSize - 1),
                                            ringNum + 1, dotsParam * 3, dotsParam);
                // Частица слева
                // Поиск точки в массиве смежных частиц по периметру
                else if(ringNum < chainSize)
                    initialAnalizeTor(ringNum, ringNum + chainSize, chainSize * chainSize - chainSize + ringNum, 0, dotsParam * 2);
                // Поиск точки в массиве смежных частиц внутри колец
                else if(ringNum > chainSize - 1)
                    initialAnalizeDuplicate(ringNum, ((ringNum - chainSize) / (chainSize * (chainSize - 1)) + (ringNum - chainSize) * (chainSize - 1) +
                                            ((ringNum - chainSize) / chainSize)) % (chainSize * (chainSize - 1)), ringNum - chainSize, 0, dotsParam * 2);
                // Частица сверху
                // Поиск точки в массиве смежных частиц по периметру
                else if(ringNum % chainSize == 0)
                    initialAnalizeTor(ringNum, ringNum / chainSize, ringNum + chainSize - 1, dotsParam, dotsParam * 3);
                // Поиск точки в массиве смежных частиц внутри колец
                else if(ringNum % chainSize != 0)
                    initialAnalizeDuplicate(ringNum, (((ringNum - 1) % chainSize) * chainSize + ((ringNum - 1) / chainSize)) + chainSize * (chainSize - 1),
                        ringNum - 1, dotsParam, dotsParam * 3);
            }
        }

        private void startButton_Click_1(object sender, EventArgs e) // Считывает параметры, введенные пользователем
        {
            uint time=0;
           
            bool correctParameters = true;
            warningLabel.ForeColor = Color.Red;
            if (dotsParamField.Text != "") // Cчитать количество точек
            {
                try
                {
                    dotsParam = uint.Parse(dotsParamField.Text);
                }
                catch (FormatException)
                {
                    dotsParamField.Text = "";
                    correctParameters = false;
                }
            }
            else
                dotsParamField.Text = "3";
            if (chainSizeField.Text != "") // Считать размер кольчуги
            {
                try
                {
                    chainSize = uint.Parse(chainSizeField.Text);
                    warningLabel.Visible = false;
                }
                catch (FormatException)
                {
                    chainSizeField.Text = "";
                    correctParameters = false;
                }
            }
            else
                chainSizeField.Text = "4";
            if (clusterLengthField.Text != "") // Считать длину кластера
            {
                try
                {
                    clustersLength = uint.Parse(clusterLengthField.Text);
                    warningLabel.Visible = false;
                }
                catch (FormatException)
                {
                    clusterLengthField.Text = "";
                    correctParameters = false;
                }
            }
            else
                clusterLengthField.Text = "5";
            if (DiamCircle.Text != "")// В случае если поле заполненно
            {
                try
                {
                    diameterBlackCircle = uint.Parse(DiamCircle.Text);
                    warningLabel.Visible = false;
                }
                catch (FormatException)
                {
                    DiamCircle.Text = "";
                    correctParameters = false;
                }
            }
            else
                DiamCircle.Text = "70";

            if (DiamParticle.Text != "")// В случае если поле заполненно
            {
                try
                {
                    diameterParticle = uint.Parse(DiamParticle.Text);
                    warningLabel.Visible = false;
                }
                catch (FormatException)
                {
                    DiamParticle.Text = "";
                    correctParameters = false;
                }
            }
            else
                DiamParticle.Text = "6";

            if (timeBox.Text != "")// В случае если поле заполненно
            {
                try
                {
                    time= uint.Parse(timeBox.Text);
                    warningLabel.Visible = false;
                }
                catch (FormatException)
                {
                    timeBox.Text = "";
                    
                    correctParameters = false;
                }
                timeBox.Enabled = false;
            }
            else
                timeBox.Text = "40";

            if (!correctParameters) // если неверный(-е) параметр, вывести сообщение об ошибке
                warningLabel.Visible = true;
            if (correctParameters) // если правильные параметры, начать работу
            {
                setGlobalVariables(); // Задает глобальные параметры
                initializeChain();  // Задает координаты объектов
                initialAnalize();   // Определяет состояния смежных частиц
                processParticles();  // Обработка частиц
                if (!startButtonClicked)
                {
                    
                    Timer timer = new Timer();
                    
                    timer.Interval = (int)time;
                   
                    // Интервал таймера 
                    timer.Tick += new EventHandler(timer1_Tick); // Задание обработки таймера 
                    timer.Start();                               // Запуск таймера
                }
                startButtonClicked = true;
            }
        }
    }
}