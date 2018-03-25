using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace MicroMouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Author: Udayan Baidya
    /// Year: 2017
    /// Computer Science and Engineering , NIT Agartala
    /// </summary>

    struct par
    {
        public int u;
        public int v;
    };


    public partial class MainWindow : Window
    {
        int val1, val2,but,countObs=0,countBot=0,countGoal=0,MousePosR,MousePosC,GoalPosR,GoalPosC,milliseconds,edge;
        int[,] myGrid;
        int[,] preprocess;
        int[,] visited;
        par[,] parent;
        Random rand1, rand2, rand3;
        Dictionary<int, Rectangle> _rectDict = new Dictionary<int, Rectangle>();
        Rectangle rect;
        private void StartPlaying(object sender, RoutedEventArgs e)
        {
            if (countBot >= 1 && countGoal >= 1)
            {
                preprocess[GoalPosR, GoalPosC] = 1;
                startPreprocessing(GoalPosR + 1, GoalPosC, 1);
                startPreprocessing(GoalPosR - 1, GoalPosC, 1);
                startPreprocessing(GoalPosR, GoalPosC + 1, 1);
                startPreprocessing(GoalPosR, GoalPosC - 1, 1);
                preprocess[GoalPosR, GoalPosC] = 0;
                /// End of preprocessing
                startMouseMovement(MousePosR, MousePosC, 1);
            }
            else
            {
                MessageBox.Show("Please insert 1 Mouse and 1 Cheese piece !","Attention");
            }
        }

        private async void startMouseMovement(int R, int C,int dir)
        {
            if(R==GoalPosR && C == GoalPosC)
            {
                MessageBox.Show("The Mouse has reached it's destination","Congratulations !");
                return;
            }
                int min = 100000;
            int minR = R;
            int minC = C;
            int mindirec=dir;
            int direc;
                if(checkValidMouse(R+1,C))
            {
                direc = 1;
                if (preprocess[R+1,C]<min)
                {
                    min = preprocess[R + 1, C];
                    minR = R + 1;
                    minC = C;
                    mindirec = direc;
                }

                if (preprocess[R+1, C] == min && dir == direc)
                {
                    minR = R+1;
                    minC = C;
                    mindirec = direc;
                }

            }
            if (checkValidMouse(R - 1, C))
            {
                direc = 2;
                if (preprocess[R - 1, C] < min)
                {
                    min = preprocess[R - 1, C];
                    minR = R - 1;
                    minC = C;
                    mindirec = direc;

                }
                if (preprocess[R-1, C ] == min && dir == direc)
                {
                    minR = R-1;
                    minC = C ;
                    mindirec = direc;
                }
            }
            if (checkValidMouse(R , C+1))
            {
                direc = 3;
                if (preprocess[R , C+1] < min)
                {
                    min = preprocess[R , C+1];
                    minR = R ;
                    minC = C+1;
                    mindirec = direc;

                }
                if (preprocess[R, C + 1] == min && dir == direc)
                {
                    minR = R;
                    minC = C + 1;
                    mindirec = direc;
                }

            }
            if (checkValidMouse(R , C-1))
            {
                direc = 4;
                if (preprocess[R , C-1] < min)
                {
                    min = preprocess[R , C-1];
                    minR = R ;
                    minC = C-1;
                    mindirec = direc;
                }
                if(preprocess[R,C-1]==min && dir == direc)
                {
                    minR = R;
                    minC = C - 1;
                    mindirec = direc;
                }
            }

            if(minR==R && minC == C)
            {
                MessageBox.Show("The Mouse Couldn't Reach it's Destination","Sorry");
                return;
            }

             rect = _rectDict[R * val2 + C];
            
            rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
            
            

             rect = _rectDict[minR * val2 + minC];
            
            rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.DeepSkyBlue);

            milliseconds = 1000;
            await Task.Delay(milliseconds);


            startMouseMovement(minR, minC,mindirec);
        }

        private bool checkValidMouse(int R, int C)
        {
            if (R < val1 && R >= 0 && C < val2 && C >= 0 && myGrid[R, C] != 1)
                return true;
            else
                return false;
        }

        private void startPreprocessing(int R,int C,int count)
        {
            if (checkValid(R,C,count))
            {
                if(!(R==MousePosR && C == MousePosC))
                {
                    
                    preprocess[R, C] = count;
                    
                        
                    startPreprocessing(R + 1, C, count + 1);
                    
                        
                    startPreprocessing(R - 1, C, count + 1);
                   
                    startPreprocessing(R, C + 1, count + 1);
                
                    startPreprocessing(R, C - 1, count + 1);
                   
                }
                preprocess[R, C] = count;
            }
        }

        private void RandomizeObstacles(object sender, RoutedEventArgs e)
        {
            myGrid = new int[val1, val2];
            visited = new int[val1, val2];
            parent = new par[val1, val2];
            for (int i = 0; i < val1; i++)
            {
                for (int j = 0; j < val2; j++)
                {
                    myGrid[i, j] = 0;
                    visited[i, j] = 0;
                    parent[i, j].u = 0;
                    parent[i, j].v = 0;
                }
            }

            int obsrow, obscol;
            int num = val1 * val2;
            for (int i = 0; i < num; i++)
            {
                rect = _rectDict[i];
                rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
            }
           num *=  3;
            num /= 4;
           
            countObs = rand1.Next(1, (num+1));
            int k = countObs;
            for(int i = 1; i <= k; i++)
            {
                obsrow = rand2.Next(val1);
                obscol = rand3.Next(val2);
                if (myGrid[obsrow,obscol] == 1)
                   countObs--;
                else
                    if(myGrid[obsrow, obscol] == 0)
                {
                    myGrid[obsrow, obscol] = 1;
                    rect = _rectDict[obsrow * val2 + obscol];
                    rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.IndianRed);
                }
            }

            for(int i=0;i< val1; i++)
            {
                for(int j=0;j< val2; j++)
                {
                    if(myGrid[i,j]==1 && visited[i, j] == 0)
                    {
                        
                        dfs(i, j);
                    }
                }
            }
            
             
        }


        private void dfs(int i, int j)
        {
            if (isNearEdge(i, j))
            {
                if (edge == 1)
                {
                    removeCycle(i, j);
                    edge = 0;
                }
                else
                edge = 1;
            }
            visited[i, j] = 1;
            checkDfs(i - 1, j - 1,i,j);
            checkDfs(i - 1, j,i,j);
            checkDfs(i - 1, j + 1,i,j);
            checkDfs(i, j - 1,i,j);
            checkDfs(i, j + 1,i,j);
            checkDfs(i + 1, j - 1,i,j);
            checkDfs(i + 1, j,i,j);
            checkDfs(i + 1, j + 1,i,j);
           
        }

        private bool isValidObs(int R, int C)
        {
            if (R < val1 && R >= 0 && C < val2 && C >= 0)
                return true;
            else
                return false;
        }

        private bool isNearEdge(int R, int J)
        {
            if (!isValidObs(R - 1, J - 1) || !isValidObs(R - 1, J) || !isValidObs(R-1,J+1) || !isValidObs(R,J-1) || !isValidObs(R,J+1) || !isValidObs(R+1,J-1) || !isValidObs(R+1, J) || !isValidObs(R+1,J+1) )
                return true;
            else
                return false;
        }

        private void checkDfs(int i, int j,int ui,int vi)
        {
            if (isValidObs(i, j))
            {
                if (myGrid[i, j] == 1 && visited[i, j] == 0)
                {
                    parent[i, j].u = ui;
                    parent[i, j].v = vi;
                    dfs(i, j);

                }
                else if (myGrid[i, j] == 1 && visited[i, j] == 1)
                {
                    if (!(parent[ui, vi].u == i && parent[ui, vi].v == j))
                    {
                        removeCycle(ui, vi);
                    }
                }
            }
        }

        

        private void removeCycle(int ui, int vi)
        {
            myGrid[ui, vi] = 0;
            rect = _rectDict[ui * val2 + vi];
            rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
            if(isValidObs(ui-1,vi))
                if(myGrid[ui-1,vi]==1)
            removeTop(ui-1, vi);
            if (isValidObs(ui + 1, vi))
                if (myGrid[ui + 1, vi] == 1)
                    removeBottom(ui + 1, vi);
        }

        private void removeBottom(int ui, int vi)
        {
            myGrid[ui, vi] = 0;
            rect = _rectDict[ui * val2 + vi];
            rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
            if (isValidObs(ui + 1, vi))
                if (myGrid[ui + 1, vi] == 1)
                    removeBottom(ui + 1, vi);
        }

        private void removeTop(int ui, int vi)
        {
            myGrid[ui, vi] = 0;
            rect = _rectDict[ui * val2 + vi];
            rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
            if (isValidObs(ui - 1, vi))
                if (myGrid[ui - 1, vi] == 1)
                    removeTop(ui - 1, vi);
        }

        private bool checkValid(int R,int C,int count)
        {
            if (R < val1 && R >= 0 && C < val2 && C >= 0 && myGrid[R, C] != 1)
            {
                if ( count < preprocess[R, C])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public MainWindow()
        {
            rand2 = new Random();
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            val1 = Convert.ToInt32(e.NewValue);
            string msg = String.Format("Current value: {0}", val1);
            this.textBlock1.Text = msg;
   
        }

        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
             val2 = Convert.ToInt32(e.NewValue);
            string msg = String.Format("Current value: {0}", val2);
            this.textBlock2.Text = msg;
            
        }

        private void onObstacleClick(object sender, RoutedEventArgs e)
        {
            but = 1;
        }

        private void onBotClick(object sender, RoutedEventArgs e)
        {
            but = 2;
        }

        private void onGoalClick(object sender, RoutedEventArgs e)
        {
            but = 3;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(AnswerGrid);
            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            
            foreach (var rowDefinition in AnswerGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            
            foreach (var columnDefinition in AnswerGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }
            if (but == 1)
            {
                 rect = _rectDict[row * val2 + col];
                if (myGrid[row,col] == 0)
                {
                    rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.IndianRed);
                    myGrid[row, col] = 1;
                    countObs++;
                }
                else if (myGrid[row,col] == 1)
                {
                    rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
                    myGrid[row,col] = 0;
                    countObs--;
                }
            }
            else if (but == 2)
            {
                rect = _rectDict[row * val2 + col];
                if (countBot < 1 && myGrid[row,col]!=1)
                {
                     
                    rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.DeepSkyBlue);
                    MousePosR = row;
                    MousePosC = col;
                    countBot++;
                }
                else if (countBot == 1 && row==MousePosR && col==MousePosC)
                {
                    rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
                    MousePosR = 0;
                    MousePosC = 0;
                    countBot--;
                }
            }
            else if (but == 3)
            {
                rect = _rectDict[row * val2 + col];
                if (countGoal < 1 && myGrid[row, col] != 1)
                {
                     
                    rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
                    GoalPosR = row;
                    GoalPosC = col;
                    countGoal++;
                }
                else if (countGoal == 1 && row==GoalPosR && col==GoalPosC)
                {
                    rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
                    GoalPosR = 0;
                    GoalPosC = 0;
                    countGoal--;
                }
            }

        }

        private void GridGen(object sender, RoutedEventArgs e)
        {
            AnswerGrid.Children.Clear();
            AnswerGrid.RowDefinitions.Clear();
            AnswerGrid.ColumnDefinitions.Clear();
            _rectDict.Clear();
            countObs = 0;
            countBot = 0;
            countGoal = 0;
            createGrid();
            rand1 = new Random();
           
            rand3 = new Random();
        }


        private void createGrid()
        {
            for (int i = 0; i < val1; i++)
            {
                RowDefinition row = new RowDefinition();
                
                AnswerGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < val2; i++)
            {
                AnswerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            AnswerGrid.ShowGridLines = true;
            for(int i=0;i< val1; i++)
            {
                for(int j=0;j< val2; j++)
                {
                    rect = new Rectangle();
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);
                    rect.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
                    AnswerGrid.Children.Add(rect);
                    _rectDict[i * val2 + j] = rect;
                }
            }
            myGrid = new int[val1, val2];
            for (int i = 0; i < val1; i++)
                for (int j = 0; j < val2; j++)
                    myGrid[i, j] = 0;

            preprocess = new int[val1, val2];
            for (int i = 0; i < val1; i++)
                for (int j = 0; j < val2; j++)
                    preprocess[i, j] = 100000;


        }
    }
}
