{{- /*
Return chart full name
*/ -}}
{{- define "companyservice.fullname" -}}
{{- printf "%s" (default .Chart.Name .Values.nameOverride) -}}
{{- end -}}
