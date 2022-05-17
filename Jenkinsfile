pipeline {
    agent any
    tools {
        maven 'maven 3.8.4'
        dockerTool 'docker'
    }
    stages {
        stage('Cloning git') {
            steps {
                git url: 'https://github.com/longphamit/demo-cicd-jenkins', credentialsId: 'github'
            }
        }
        stage('Build Maven') {
            steps {
                sh 'mvn clean install -DskipTests=true '
            }
        }
        stage('Build Image') {
            steps {
                sh "docker build -t longpc/demo-cicd-spring-pipeline ."
            }
        }
        stage('Test') {
            steps {
                echo 'Testing..'
            }
        }
        stage("Login dockerhub") {
            steps {
               withCredentials([usernamePassword(credentialsId: 'docker-hub', passwordVariable: 'pass', usernameVariable: 'user')]) {
                    sh "docker login -u $user -p $pass"
               }
            }
        }
        stage("Push Image") {
            steps {
               sh "docker push longpc/demo-cicd-spring-pipeline"
            }
        }
    }
}