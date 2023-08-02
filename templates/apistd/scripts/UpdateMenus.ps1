<# 
���ű�������ͬ��ǰ��ά����menus.json�����ݿ�
���ű���ǰ�˿�����Աִ�У�ִ��ǰ��ȷ��·����url����ʵ�����
���Ҫ����������������Я������ production
json��ʽʾ��:
[
  {
    "name": "ϵͳ����",
    "accessCode": "80000",
    "menuType": 0,
    "children": []
  }
]
#>

[CmdletBinding()]
param (
  [Parameter()]
  [String]
  $Environment
)
$PSDefaultParameterValues['*:Encoding'] = 'utf8'

$location = Get-Location
cd ./src
# ����ǰ��menus.json·��
$content = Get-Content .\json\menus.json  -Encoding UTF8
$url = 'http://localhost:5002'

try {
  if ($Environment.ToLower() -eq 'production') {
    # ��������������ַ
    $url = 'https://production.com'
    Write-Host "production"
  }
  
  $url = $url + '/api/admin/SystemMenu/sync/MyProjectNameDefaultKey'
  Write-Host 'request:'$url;
  $res = Invoke-RestMethod -Method 'Post' -Uri $url -Body ($content) -ContentType "application/json;charset=utf-8" 
  Write-Host $res
}
catch [System.Exception] { 
  Write-Error $_
}

cd $location
