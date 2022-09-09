#version 330 core
out vec4 FragColor;
uniform vec3 color;
void main(){
	FragColor = vec4(color,1.0f);//vec4(0.0f,0.0f,0.0f,1.0f); 
}