# MergePDFFunctionApp
Azure Function for merging PDFs online.

Pass a JSON containing a list of PDF files to merge (Base64 content) and the function will give you as output the Base64 content of the merged file.

**Usage:**

```

POST YOURFUNCTIONURL/api/MergePDF
Content-Type: application/json

[
 {
   
   "Name": "Invoice102404.pdf",
    "Base64Content": "BASE64CONTENT_DOCUMENT_1"
 },
 {
 
   "Name": "Invoice102405.pdf",
    "Base64Content": "BASE64CONTENT_DOCUMENT_2"
 }
]
```
