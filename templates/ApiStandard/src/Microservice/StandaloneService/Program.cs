var builder = WebApplication.CreateBuilder(args);
// 1 ���Ĭ�����
builder.AddDefaultWebServices();
// 2 ע�������Web��������
builder.AddDefaultWebServices();
// 3 �����Զ���ѡ�����

WebApplication app = builder.Build();

app.UseDefaultWebServices();
app.Run();
