#version 330 core
out vec4 FragColor;
in vec2 TexCoord;
uniform sampler2D Texttexture;
uniform vec3 color;
void main(){
	FragColor = vec4(texture(Texttexture,TexCoord).a) * vec4(color,1.0);
}