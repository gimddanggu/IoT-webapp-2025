using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using WpfTodoListApp.Models;

namespace WpfTodoListApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        HttpClient client = new HttpClient();
        TodoItemsCollection todoItems = new TodoItemsCollection();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var comboPairs = new List<KeyValuePair<string, int>> {
                new KeyValuePair<string, int>("완료", 1),
                new KeyValuePair<string, int>("미완료", 0),
            };
            CboIsComplete.ItemsSource = comboPairs;
            // RestAPI 호출 준비
            client.BaseAddress = new System.Uri("http://localhost:1656");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 데이터 가져오기
            GetDatas();
        }

        private async Task GetDatas()
        {
            // /api/TodoItems GET 메서드 호출
            GridTodoItems.ItemsSource = todoItems;

            try //API 호출
            {
                // http://localhost:1656/api/TodoItems
                HttpResponseMessage? response = await client.GetAsync("/api/TodoItems");
                response.EnsureSuccessStatusCode(); // 상태코드 확인

                var items = await response.Content.ReadAsAsync<IEnumerable<TodoItem>>();
                todoItems.CopyFrom(items); // ObservableCollection으로 형변환

                // 성공메시지
                await this.ShowMessageAsync("result", "로드 성공!");

            }
            catch (Exception ex)
            {
                // 예외메시지
                await this.ShowMessageAsync("API 호출 에러", ex.Message);
            }
        }

        // async시 Task가 리턴값이지만 버튼클릭 이벤트메서드와 연결시는 Task -> void 로 변경해줘야 연결 가능
        private async void BtnInsert_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var todoItem = new TodoItem
                {
                    Id = 0, // Id는 테이블에서 자동생성 AutoIncrement
                    Title = TxtTitle.Text,
                    TodoDate = Convert.ToDateTime(DtpTodoDate.SelectedDate).ToString("yyyyMMdd"),
                    IsComplete = Convert.ToBoolean(CboIsComplete.SelectedValue)
                };
                // 데이터 입력확인
                //Debug.WriteLine(todoItem.Title);

                // POST 메서드 API 호출
                var response = await client.PostAsJsonAsync("/api/TodoItems", todoItem);
                response.EnsureSuccessStatusCode();

                GetDatas();
                // 입력양식 초기화
                InitControls();

                // 입력양식 초기화
                TxtId.Text = String.Empty;
                TxtTitle.Text = String.Empty;
                DtpTodoDate.Text = String.Empty;
                CboIsComplete.Text = String.Empty;
            }
            catch (Exception ex)
            {
                // 예외메시지
                await this.ShowMessageAsync("API 호출 에러", ex.Message);
            }
        }

        private async void GridTOdoItems_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            try
            {

                //await this.ShowMessageAsync("클릭", "클릭확인");
                var Id = (GridTodoItems.SelectedItem as TodoItem)?.Id; // ?. > Null일 경우 생겨도 예외발생안함
                
                if (Id == null) return; // 이 구문을 만나야 아래 로직이 실행안됨
                
                // /api/todoItems/{Id} GET 메서드 API 호출
                var response = await client.GetAsync($"/api/TodoItems/{Id}");
                response.EnsureSuccessStatusCode();

                var item = await response.Content.ReadAsAsync<TodoItem>();

                TxtId.Text = item.Id.ToString();
                TxtTitle.Text = item.Title.ToString();
                DtpTodoDate.SelectedDate = DateTime.Parse(item.TodoDate.Insert(4, "-").Insert(7, "-"));
                CboIsComplete.SelectedValue = item.IsComplete;
            }
            catch (Exception ex) 
            {
                // 예외메시지
                await this.ShowMessageAsync("API 호출 에러", ex.Message);
            }
        }

        private async void BtnUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var todoItem = new TodoItem
                {
                    Id = Convert.ToInt32(TxtId.Text),
                    Title = TxtTitle.Text,
                    TodoDate = Convert.ToDateTime(DtpTodoDate.SelectedDate).ToString("yyyyMMdd"),
                    IsComplete = Convert.ToBoolean(CboIsComplete.SelectedValue)
                };

                var response = await client.PutAsJsonAsync($"/api/TodoItems/{todoItem.Id}", todoItem);
                response.EnsureSuccessStatusCode();

                GetDatas();
                // 입력양식 초기화
                InitControls();


            }
            catch (Exception ex)
            {

                // 예외메시지
                await this.ShowMessageAsync("API 호출 에러", ex.Message);
            }
        }

        private void InitControls()
        {
            GetDatas();

            // 입력양식 초기화
            TxtId.Text = String.Empty;
            TxtTitle.Text = String.Empty;
            DtpTodoDate.Text = String.Empty;
            CboIsComplete.Text = String.Empty;
        }

        private async void BtnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var Id = Convert.ToInt32(TxtId.Text); // 삭제는 Id만 파라미터로 전송

                var response = await client.DeleteAsync($"/api/TodoItems/{Id}");
                response.EnsureSuccessStatusCode();

                GetDatas();
                InitControls();
            }
            catch (Exception ex)
            {
                // 예외메시지
                await this.ShowMessageAsync("API 호출 에러", ex.Message);
            }
        }
    }
}