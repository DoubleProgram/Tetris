#version 330 core
out vec4 FragColor;
in vec2 TexCoord;
uniform sampler2D Bricktexture;
void main(){
	FragColor = texture(Bricktexture,TexCoord);
}